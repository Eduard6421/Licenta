using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KinematicEntity : MonoBehaviour
{
    private delegate void UpdateHandler();

    [SerializeField]
    private Utilities.Jobs AgentJob;
    private int GroupId;
    private bool SystemReady;

    [SerializeField]
    private float PathUpdateTime = 0.5f;
    private float CurrentTime = 0f;

    private NavMeshAgent NavAgent;
    private Animator RobotAnimator;

    private BlendedSteeringBehaviour steeringType;
    private SteeringOutput steering;
    private SteeringOutput oldSteering;

    private float MaxSpeed;
    private float MaxRotation;

    private Vector3 velocity;
    private float rotation;

    private GameObject Target;
    private KinematicEntity TargetKinematic;
    private Vector3 TargetVelocity;
    private float TargetRotation;

    private Goal CurrentGoal;
    private Vector3 CurrentTargetPosition;
    private float CurrentTargetOrientation;
    private float CurrentInteractionTime;

    private NavMeshPath CurrentPath;

    private GoalManager GoalMaster;
    private BuilderManager BuilderMaster;
    private BehaviourBuilder AgentBehaviourBuilder;

    private UpdateHandler AgentUpdate;

    IEnumerator AgentStartSleep()
    {
        enabled = false;
        yield return new WaitUntil(() => GoalMaster.IsReady() == true);
        enabled = true;
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public float GetRotation()
    {
        return rotation;
    }

    public void SetBehaviour(BlendedSteeringBehaviour newBehaviour, float maxSpeed, float maxRotation, GameObject target, List<Vector3> characterPath)
    {
        this.steeringType = newBehaviour;
        this.MaxSpeed = maxSpeed;
        this.MaxRotation = maxRotation;
        this.Target = target;
    }

    public void SetNewGroup(int newGroup)
    {
        this.GroupId = newGroup;
    }

    public void SetNewGoal(Goal newGoal)
    {
        this.CurrentGoal = newGoal;
    }

    //Does not make use of object interaction
    public void PatrolGoal()
    {
        float distance;
        Vector3 targetPosition = CurrentGoal.GetCurrentTargetPosition();

        distance = (targetPosition - transform.position).magnitude;

        CurrentTime += Time.deltaTime;

        if (distance < 0.5f)
        {
            if (steeringType != null)
            {
                steeringType = null;
                CurrentInteractionTime = 0;
            }
            else
            {
                CurrentInteractionTime += Time.deltaTime;

                if (CurrentInteractionTime > CurrentGoal.GoalActionTime)
                {
                    CurrentGoal.UpdatePatrolTarget();
                }
            }
        }
        else if (steeringType == null || CurrentTargetPosition != targetPosition || CurrentTime > PathUpdateTime)
        {
            NavMeshHit hit;
            NavMesh.SamplePosition(targetPosition, out hit, 3f, NavMesh.AllAreas);

            CurrentTime = 0;

            targetPosition = hit.position;

            NavAgent.CalculatePath(targetPosition, CurrentPath);
            NavAgent.SetPath(CurrentPath);

            CurrentTargetPosition = targetPosition;
            CurrentTargetOrientation = 0;

            List<Vector3> cornerArray = new List<Vector3>(CurrentPath.corners);

            steeringType = AgentBehaviourBuilder.walkingSteering(cornerArray, new List<GameObject>());
        }

    }
    //Makes use of object interaction
    public void InteractionGoal()
    {
        float distance;

        Vector3 targetPosition = CurrentGoal.GetCurrentTargetPosition();

        distance = (targetPosition - transform.position).magnitude;

        CurrentTime += Time.deltaTime;

        if (distance < 2f)
        {
            NavAgent.isStopped = true;

            if (steeringType != null)
            {
                steeringType = null;
                CurrentInteractionTime = 0;
            }
            else
            {
                CurrentInteractionTime += Time.deltaTime;

                if (CurrentInteractionTime > CurrentGoal.GoalActionTime)
                {
                    Debug.Log("Finished");
                    CurrentGoal = null;
                }
            }
        }
        else if (steeringType == null || CurrentTargetPosition != targetPosition || CurrentTime > PathUpdateTime)
        {
            NavMeshHit hit;
            NavMesh.SamplePosition(targetPosition, out hit, 3f, NavMesh.AllAreas);

            CurrentTime = 0;

            Target = CurrentGoal.GetCurrentTargetObject();
            targetPosition = hit.position;

            NavAgent.CalculatePath(targetPosition, CurrentPath);
            NavAgent.SetPath(CurrentPath);

            CurrentTargetPosition = targetPosition;
            CurrentTargetOrientation = 0;

            List<Vector3> cornerArray = new List<Vector3>(CurrentPath.corners);

            steeringType = AgentBehaviourBuilder.walkingSteering(cornerArray, new List<GameObject>());
        }
    }

    public void WaitGoal()
    {

        CurrentTime += Time.deltaTime;

        if(CurrentTime > CurrentGoal.GoalActionTime)
        {
            Debug.Log("Finished");
            CurrentGoal = null;
        }
    }

    public void SetAgentType(Utilities.Jobs jobType,int groupId)
    {
        this.AgentJob = jobType;
        this.GroupId = groupId;

    }



    void SetAgentUpdateType()
    {
        if (this.AgentJob == Utilities.Jobs.GroupMember)
            this.AgentUpdate = GroupAgentUpdate;
        else
            this.AgentUpdate = IndividualAgentUpdate;
    }
    void SetAgentBuilderType()
    {
        BuilderMaster = BuilderManager.GetInstance();

        switch (AgentJob)
        {
            case Utilities.Jobs.Civilian:
                AgentBehaviourBuilder = BuilderMaster.GetCivilianBuilder();
                break;
            case Utilities.Jobs.Patrolman:
                AgentBehaviourBuilder = BuilderMaster.GetCivilianBuilder();
                break;
            case Utilities.Jobs.GroupMember:
                AgentBehaviourBuilder = BuilderMaster.GetCivilianBuilder();
                break;
            default:
                break;
        }

    }

    void IsKinematicTarget()
    {
        if (TargetKinematic != null)
        {
            TargetVelocity = TargetKinematic.GetVelocity();
            TargetRotation = TargetKinematic.GetRotation();
        }
        else
        {
            TargetVelocity = Vector3.zero;
            TargetRotation = 0;
        }
    }
    void UpdateSteering()
    {
        if (steering.linearSpeed == Vector3.zero)
        {
            velocity = Vector3.zero;
            RobotAnimator.SetFloat("speed", 0);
        }
        else
        {
            velocity += steering.linearSpeed;
            //Debug.Log(NavAgent.desiredVelocity);
            steeringType.setRVOVelocity(NavAgent.desiredVelocity);

            RobotAnimator.SetFloat("speed", NavAgent.velocity.magnitude);

        }

        if (steering.angularSpeed == 0)
        {
            rotation = 0;
            RobotAnimator.SetFloat("rotation", 0);
        }
        else
        {
            rotation += steering.angularSpeed;

            if (rotation > MaxRotation)
            {
                rotation = Mathf.Abs(rotation) / rotation * MaxRotation;
            }
            NavAgent.angularSpeed += rotation;

            if (NavAgent.angularSpeed > MaxRotation)
            {
                NavAgent.angularSpeed = Mathf.Abs(NavAgent.angularSpeed) / NavAgent.angularSpeed * MaxRotation;
            }


            RobotAnimator.SetFloat("rotation", NavAgent.angularSpeed);
        }


    }

    //Currently only restaring the interaction time;
    void RestartAgentGoal()
    {
        CurrentTime = 0;
    }

    void Start()
    {
        velocity = Vector3.zero;
        rotation = 0f;
        CurrentPath = new NavMeshPath();
        TargetKinematic = null;

        RobotAnimator = this.GetComponent<Animator>();
        NavAgent = this.GetComponent<NavMeshAgent>();
        GoalMaster = GoalManager.GetInstance();


        SetAgentBuilderType();
        SetAgentUpdateType();
        AgentStartSleep();

    }

    void Update()
    {
        AgentUpdate();
    }

    void GroupAgentUpdate()
    {
        if (CurrentGoal != null)
        {
            CurrentGoal.GoalFunction();
            if (steeringType != null)
            {
                IsKinematicTarget();

                steering = steeringType.GetSteering(this.transform.position, this.transform.eulerAngles.y, velocity,
                    rotation, CurrentTargetPosition, CurrentTargetOrientation, TargetVelocity,
                    TargetRotation);
                UpdateSteering();
            }
        }
        else
        {
            Debug.Log("Group Request");
            RestartAgentGoal();
            CurrentGoal = GoalMaster.RequestBehaviour(this.gameObject, AgentJob, GroupId);

        }



    }

    void IndividualAgentUpdate()
    {
        if (CurrentGoal != null)
        {
            CurrentGoal.GoalFunction();
            if (steeringType != null)
            {
                IsKinematicTarget();

                steering = steeringType.GetSteering(this.transform.position, this.transform.eulerAngles.y, velocity,
                    rotation, CurrentTargetPosition, CurrentTargetOrientation, TargetVelocity,
                    TargetRotation);
                UpdateSteering();
            }
        }
        else
        {
            Debug.Log("Individual Request");
            RestartAgentGoal();
            CurrentGoal = GoalMaster.RequestBehaviour(this.gameObject, AgentJob,GroupId);
        }

    }

}
