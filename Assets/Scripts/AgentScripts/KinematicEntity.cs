using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Profiling;

public class KinematicEntity : MonoBehaviour
{
    private delegate void UpdateHandler();

    [SerializeField]
    private Utilities.Jobs AgentJob;
    private int GroupId;
    private bool SystemReady;

    [SerializeField]
    private float PathUpdateTime = 1f;
    private float CurrentTime = 0f;

    [SerializeField]
    private Material TrailMaterial;

    private NavMeshAgent NavAgent;
    private Animator RobotAnimator;
    private EmergencyLocator ExitLocator;
    private TrailRenderer AgentTrail;
    private GameObject TrailObject;

    private BlendedSteeringBehaviour steeringType;
    private SteeringOutput steering;
    private SteeringOutput oldSteering;

    private float MaxSpeed;
    private float MaxUncapSpeed;
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
    private bool lastTarget = false;

    private NavMeshPath CurrentPath;

    private GoalManager GoalMaster;
    private BuilderManager BuilderMaster;
    private ReportScript ReportScript;
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
        this.MaxUncapSpeed = maxSpeed;
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
        steeringType = null;
    }

    public void CapSpeed()
    {
        this.MaxSpeed = 0.5f;

    }


    public void UncapSpeed()
    {
        if(MaxSpeed < MaxUncapSpeed)
        {
            this.MaxSpeed += 0.1f;
        }
    }



    //Does not make use of object interaction
    public void MoveGoal()
    {
        float distance;
        Vector3 targetPosition = CurrentGoal.GetCurrentTargetPosition();

        distance = (targetPosition - transform.position).magnitude;

        CurrentTime += Time.deltaTime;

        if (distance < 0.5f)
        {
            if (steeringType == null)
            {
                steeringType = null;
                CurrentInteractionTime = 0;
                NavAgent.avoidancePriority = 40;
            }
            else
            {
                CurrentInteractionTime += Time.deltaTime;
                NavAgent.avoidancePriority = 40;

                if (CurrentInteractionTime > CurrentGoal.GoalActionTime)
                {
                    CurrentGoal = null;
                }

            }
        }
        else if (steeringType == null || CurrentTime > PathUpdateTime)
        {
            NavMeshHit hit;
            NavMesh.SamplePosition(targetPosition, out hit, Mathf.Infinity, NavMesh.AllAreas);
            CurrentTime = 0;

            //Debug.Log("Target position" + targetPosition);
            targetPosition = hit.position;

            NavAgent.CalculatePath(targetPosition, CurrentPath);
            NavAgent.SetPath(CurrentPath);
            NavAgent.avoidancePriority = 50;

            List<Vector3> cornerArray = new List<Vector3>(CurrentPath.corners);

            steeringType = AgentBehaviourBuilder.walkingSteering(cornerArray, new List<GameObject>());
            
        }
        NavAgent.velocity = velocity;
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
                NavAgent.avoidancePriority = 40;
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
        else if (steeringType == null || CurrentTime > PathUpdateTime)
        {
            NavMeshHit hit;
            NavMesh.SamplePosition(targetPosition, out hit, 3f, NavMesh.AllAreas);

            CurrentTime = 0;

            targetPosition = hit.position;

            NavAgent.CalculatePath(targetPosition, CurrentPath);
            NavAgent.SetPath(CurrentPath);
            NavAgent.avoidancePriority = 50;

            CurrentTargetPosition = targetPosition;
            CurrentTargetOrientation = 0;

            List<Vector3> cornerArray = new List<Vector3>(CurrentPath.corners);

            steeringType = AgentBehaviourBuilder.walkingSteering(cornerArray, new List<GameObject>());
        }
        NavAgent.velocity = velocity;

    }
    //Makes use of object interaction
    public void InteractionGoal()
    {
        float distance;

        Vector3 targetPosition = CurrentGoal.GetCurrentTargetPosition();

        distance = (targetPosition - transform.position).magnitude;

        CurrentTime += Time.deltaTime;

        if (distance < 1.2f)
        {

                CurrentInteractionTime += Time.deltaTime;
                NavAgent.avoidancePriority = 40;
                if (CurrentInteractionTime > CurrentGoal.GoalActionTime)
                {
                    CurrentGoal = null;
                }
            //gameObject.transform.LookAt(targetPosition);
        }
        else if (steeringType == null || CurrentTime > PathUpdateTime)
        {
            NavMeshHit hit;
            NavMesh.SamplePosition(targetPosition, out hit, 3f, NavMesh.AllAreas);

            CurrentTime = 0;

            Target = CurrentGoal.GetCurrentTargetObject();
            targetPosition = hit.position;

            NavAgent.CalculatePath(targetPosition, CurrentPath);
            NavAgent.SetPath(CurrentPath);
            NavAgent.avoidancePriority = 50;


            CurrentTargetPosition = targetPosition;
            CurrentTargetOrientation = 0;

            List<Vector3> cornerArray = new List<Vector3>(CurrentPath.corners);

            steeringType = AgentBehaviourBuilder.walkingSteering(cornerArray, new List<GameObject>());
        }
        NavAgent.velocity = velocity;
    }
    //Makes agents wait for a specified time period
    public void WaitGoal()
    {

        CurrentTime += Time.deltaTime;
        steeringType = null;

        if (CurrentTime > CurrentGoal.GoalActionTime)
        {
            NavAgent.avoidancePriority = 0;
            CurrentGoal = null;
        }
    }

    public void ExitGoal()
    {

        float distance;
        MaxSpeed = 5;
        MaxRotation = 5;
        MaxUncapSpeed = 5;

        Vector3 targetPosition = Vector3.zero;

        try
        {
            targetPosition = CurrentGoal.GetCurrentTargetPosition();
        }
        catch(Exception e)
        {
            targetPosition = CurrentGoal.GetCurrentTargetPosition();
        }
        GameObject targetObject = CurrentGoal.GetCurrentTargetObject();


        distance = (targetPosition - transform.position).magnitude;

        CurrentTime += Time.deltaTime;

        if (distance <5f && targetObject != null)
        {
            steeringType = null;
            if (targetObject != null)
            {
                if(lastTarget)
                {
                    Destroy(gameObject);
                }
                else
                {
                    lastTarget = CurrentGoal.UpdateSequentialTargetObject();
                }
            }
            else
            {
                GoalMaster.UpdateEmergency(this.gameObject);
            }
        }
        else if (steeringType == null || CurrentTime > PathUpdateTime)
        {
            CurrentTime = 0;

            NavAgent.CalculatePath(targetPosition, CurrentPath);
            NavAgent.SetPath(CurrentPath);
            NavAgent.avoidancePriority = 50;

            List<Vector3> cornerArray = new List<Vector3>(CurrentPath.corners);

            steeringType = AgentBehaviourBuilder.walkingSteering(cornerArray, new List<GameObject>());

        }
        NavAgent.velocity = velocity;

    }

    public void SetAgentType(Utilities.Jobs jobType, int groupId)
    {
        this.AgentJob = jobType;
        this.GroupId = groupId;

    }

    public void ToggleTrailRendering()
    {
        TrailObject = new GameObject();
        TrailObject.transform.position = this.transform.position;
        TrailObject.transform.parent = this.transform;
        TrailObject.tag = "Trail";
        TrailObject.name = "Trail" + this.name.Substring(5);

        AgentTrail = TrailObject.AddComponent<TrailRenderer>();
        AgentTrail.time = 100000;
        AgentTrail.widthMultiplier = 0.2f;
        AgentTrail.material = TrailMaterial;
    }


    private void OnDestroy()
    {
        if (TrailObject != null)
        {
            TrailObject.transform.parent = null;
            AgentTrail.autodestruct = true;
        }

        ReportScript.ReportDeletedAgent();

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
        if (steering.linearAcceleration == Vector3.zero)
        {
            velocity = Vector3.zero;
            RobotAnimator.SetFloat("speed", 0);
        }
        else
        {
            velocity += steering.linearAcceleration;

            if (velocity.magnitude > MaxSpeed)
            {
                velocity.Normalize();
                velocity *= MaxSpeed;
            }

            steeringType.setRVOVelocity(NavAgent.desiredVelocity);

            RobotAnimator.SetFloat("speed", NavAgent.velocity.magnitude);

        }

        if (steering.angularAcceleration == 0)
        {
            rotation = 0;
            RobotAnimator.SetFloat("rotation", 0);
        }
        else
        {

            if (rotation > MaxRotation)
            {
                rotation = Mathf.Abs(rotation) / rotation * MaxRotation;
            }
            //RobotAnimator.SetFloat("rotation", NavAgent.angularSpeed);
        }


    }

    //Currently only restaring the interaction time;
    void RestartAgentGoal()
    {
        steeringType = null;
        velocity = Vector3.zero;
        rotation = 0;
        CurrentTime = 0;
        CurrentInteractionTime = 0;

        TargetVelocity = Vector3.zero;
        TargetRotation = 0f;
        lastTarget = false;

        RobotAnimator.SetFloat("speed", velocity.magnitude);
        RobotAnimator.SetFloat("rotation", rotation);


    }

    void Start()
    {
        steering = new SteeringOutput();
        velocity = Vector3.zero;
        rotation = 0f;
        CurrentPath = new NavMeshPath();
        TargetKinematic = null;

        RobotAnimator = this.GetComponent<Animator>();
        NavAgent = this.GetComponent<NavMeshAgent>();

        GoalMaster = GoalManager.GetInstance();
        ReportScript = ReportScript.GetInstance();
        MaxSpeed = 2;
        MaxRotation = 2;
        MaxUncapSpeed = 2;

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
            //Debug.Log("Group Request");
            RestartAgentGoal();
            CurrentGoal = GoalMaster.RequestBehaviour(this.gameObject, AgentJob, GroupId, Target);

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
            //Debug.Log("Individual Request");
            CurrentGoal = GoalMaster.RequestBehaviour(this.gameObject, AgentJob, GroupId, Target);
            RestartAgentGoal();
        }

    }


}
