using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KinematicEntity : MonoBehaviour
{
    [SerializeField]
    private Utilities.Jobs AgentJob;

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
    private Vector3 CurrentTargetPosition;
    private float targetRotation;

    private Goal CurrentGoal;
    private float CurrentInteractionTime;

    private NavMeshPath CurrentPath;

    private GoalManager GoalMaster;
    private BuilderManager BuilderMaster;
    private BehaviourBuilder AgentBehaviourBuilder;

    IEnumerator AgentStartSleep()
    {
        enabled = false;
        yield return new WaitUntil(() => GoalManager.IsReady() == true);
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

    public void InteractionGoal(GameObject target, Vector3 targetPosition, float interactionTime)
    {
        float distance;
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

                if (CurrentInteractionTime > interactionTime)
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

            Target = target;
            targetPosition = hit.position;

            NavAgent.CalculatePath(targetPosition, CurrentPath);
            NavAgent.SetPath(CurrentPath);

            CurrentTargetPosition = targetPosition;

            List<Vector3> cornerArray = new List<Vector3>(CurrentPath.corners);

            steeringType = AgentBehaviourBuilder.walkingSteering( 5f, 5f, cornerArray, new List<GameObject>());
        }
    }

    

    void AgentSetBuilderType()
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
            default:
                break;
        }

    }

    void IsKinematicTarget()
    {
        if (TargetKinematic != null)
        {
            TargetVelocity = TargetKinematic.GetVelocity();
            targetRotation = TargetKinematic.GetRotation();
        }
        else
        {
            TargetVelocity = Vector3.zero;
            targetRotation = 0;
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

    void Start()
    {
        velocity = Vector3.zero;
        rotation = 0f;
        AgentJob = Utilities.Jobs.Civilian;

        RobotAnimator = this.GetComponent<Animator>();
        NavAgent = this.GetComponent<NavMeshAgent>();

        GoalMaster = GoalManager.GetInstance();

        CurrentPath = new NavMeshPath();
        TargetKinematic = null;


        AgentSetBuilderType();


        AgentStartSleep();


    }

    void Update()
    {
        if (CurrentGoal != null)
        {
            CurrentGoal.GoalFunction(CurrentGoal.InteractionObject, CurrentGoal.TargetPosition,
                CurrentGoal.InteractionTime);
            if (steeringType != null)
            {
                IsKinematicTarget();

                steering = steeringType.GetSteering(this.transform.position, this.transform.eulerAngles.y, velocity,
                    rotation, Target.transform.position, Target.transform.eulerAngles.y, TargetVelocity,
                    targetRotation);
                UpdateSteering();
            }
        }
        else
        {
            Debug.Log("Request");
            CurrentGoal = GoalMaster.RequestBehaviour(this.gameObject, AgentJob);


        }

    }

}
