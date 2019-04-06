using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KinematicEntity : MonoBehaviour
{

    private NavMeshAgent NavAgent;
    private Animator RobotAnimator;

    private ISteerable steeringType;
    private SteeringOutput steering;

    private float MaxSpeed;
    private float MaxRotation;

    private Vector3 velocity;
    private float rotation;


    GoalManager BehaviourMaster;

    [SerializeField]
    private string AgentJob = "Civillian";

    private GameObject Target;
    private KinematicEntity TargetKinematic;
    private Vector3 TargetVelocity;
    private Vector3 CurrentTargetPosition;
    private float targetRotation;


    private Goal CurrentGoal;
    private float CurrentInteractionTime;

    NavMeshPath CurrentPath;

    IEnumerator AgentStartSleep()
    {
        yield return new WaitUntil(() => GoalManager.IsReady() == true);

    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public float GetRotation()
    {
        return rotation;
    }

    public void SetBehaviour(ISteerable newBehaviour, float maxSpeed, float maxRotation, GameObject target, List<Vector3> characterPath)
    {
        this.steeringType = newBehaviour;
        this.MaxSpeed = maxSpeed;
        this.MaxRotation = maxRotation;
        this.Target = target;
    }


    private void Start()
    {
        velocity = Vector3.zero;
        rotation = 0f;

        RobotAnimator = this.GetComponent<Animator>();
        NavAgent = this.GetComponent<NavMeshAgent>();
        BehaviourMaster = GoalManager.GetInstance();
        CurrentPath = new NavMeshPath();

        enabled = false;
        AgentStartSleep();
        enabled = true;

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
            Debug.Log(NavAgent.desiredVelocity);
            velocity += NavAgent.desiredVelocity;

            if (velocity.magnitude > MaxSpeed)
            {

                velocity.Normalize();
                velocity = velocity * MaxSpeed;
            }

            NavAgent.velocity = velocity;

            RobotAnimator.SetFloat("speed", velocity.magnitude);
        }

        if (steering.angularSpeed == 0)
        {
            rotation = 0;
            RobotAnimator.SetFloat("rotation", 0);
        }
        else
        {

            rotation += steering.angularSpeed;

            if (Mathf.Abs(rotation) > MaxRotation)
            {
                rotation = rotation / Mathf.Abs(rotation);
                rotation = rotation * MaxRotation;
            }

            NavAgent.angularSpeed = rotation;
            RobotAnimator.SetFloat("rotation", rotation);
        }


    }

    public void InteractionGoal(GameObject target, Vector3 targetPosition, float interactionTime)
    {
        float distance;
        distance = (targetPosition - transform.position).magnitude;

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
        else if (steeringType == null || CurrentTargetPosition != targetPosition)
        {
            Target = target;
            NavAgent.CalculatePath(targetPosition, CurrentPath);
            NavAgent.SetDestination(targetPosition);
            CurrentTargetPosition = targetPosition;
            List<Vector3> cornerArray = new List<Vector3>(CurrentPath.corners);
            steeringType = BehaviourFactory.GetCharacterBehaviour(Utilities.Behaviour.PathFollowing, out MaxSpeed, out MaxRotation, cornerArray, new List<GameObject>());

        }


    }


    void Update()
    {
        if (CurrentGoal != null)
        {
            CurrentGoal.GoalFunction(CurrentGoal.InteractionObject, CurrentGoal.TargetPosition, CurrentGoal.InteractionTime); 
            if(steeringType != null)
            {
                this.transform.position += velocity * Time.deltaTime;
                this.transform.Rotate(Vector3.up * rotation);
                IsKinematicTarget();
                
                steering = steeringType.GetSteering(this.transform.position, this.transform.eulerAngles.y, velocity, rotation, Target.transform.position, Target.transform.eulerAngles.y, TargetVelocity, targetRotation);
                UpdateSteering();
            }
        }
        else
        {
            Debug.Log("Request");
            CurrentGoal = BehaviourMaster.RequestBehaviour(this.gameObject, AgentJob);
        }

    }
}
