using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoalManager : MonoBehaviour
{
    private static GoalManager instance = null;

    private List<GameObject> Agents;

    private HotSpotManager HotspotManagerInstance;
    private PatrolManager PatrolManagerInstance;
    private GroupManager GroupManagerInstance;


    private bool HotspotsReady = false;

    private Vector3 LeftBound;
    private Vector3 RightBound;

    public static GoalManager GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        HotspotManagerInstance = HotSpotManager.GetInstance();
        PatrolManagerInstance = PatrolManager.GetInstance();
        GroupManagerInstance = GroupManager.GetInstance();
        PlaneScript planeScript = (PlaneScript) GameObject.FindObjectOfType(typeof(PlaneScript));
        planeScript.GetBounds(out LeftBound,out RightBound);
    }


    public Goal RequestBehaviour(GameObject agent, Utilities.Jobs agentJob, int groupID, GameObject Target)
    {

        Goal newGoal;
        GoalHandler newHandler;
        Utilities.Actions actionType;
        List<GameObject> groupMembers;
        KinematicEntity memberKinematic;

        if (agentJob == Utilities.Jobs.GroupMember)
        {

            actionType = GroupManagerInstance.GetCurrentGroupAction(groupID);

            if(Target != null)
            {
                HotspotManagerInstance.UpdateHotSpot(Target.name,GroupManagerInstance.GetGroupSize(groupID));
                }


            switch (actionType)
            {

                case Utilities.Actions.Nothing:
                    {
                        // The group is doing nothing currently
                        actionType = BehaviourProbabilities.GetBehaviourType(agentJob);
                        switch (actionType)
                        {

                            case Utilities.Actions.Interact:

                                int groupSize = GroupManagerInstance.GetGroupSize(groupID);

                                GameObject hotspot = HotspotManagerInstance.getRandomHotSpot(groupSize);
                                if (hotspot == null)
                                {
                                    goto case Utilities.Actions.Move;
                                }

                                newGoal = new Goal(goalActionTime: Random.Range(5, 20), targetPositions: new List<Vector3> { hotspot.transform.position }, interactionObject: new List<GameObject> { hotspot });
                                //Set the new action of the group

                                GroupManagerInstance.SetGroupAction(groupID, Utilities.Actions.Nothing, newGoal);

                                groupMembers = GroupManagerInstance.GetGroupAgents(groupID);

                                for(int i = 0; i < groupMembers.Count; ++i)
                                {
                                    memberKinematic = groupMembers[i].GetComponent<KinematicEntity>();
                                    GoalHandler tmpHandler = memberKinematic.InteractionGoal;
                                    Goal tmpGoal = new Goal(newGoal,tmpHandler);
                                    groupMembers[i].GetComponent<KinematicEntity>().SetNewGoal(tmpGoal);
                                }

                                newHandler = agent.GetComponent<KinematicEntity>().InteractionGoal;

                                newGoal = new Goal(newGoal,goalFunction: newHandler);
                                Debug.Log("Group Interaction");
                                return newGoal;

                            case Utilities.Actions.Move:

                                Vector3 targetPosition = new Vector3();
                                targetPosition.x = Random.Range(LeftBound.x, RightBound.x);
                                targetPosition.z = Random.Range(LeftBound.z, RightBound.z);
                                NavMeshHit hit;

                                int navMeshMask = NavMesh.GetAreaFromName("Interior");

                                NavMesh.SamplePosition(targetPosition, out hit, Mathf.Infinity, navMeshMask);

                                targetPosition = hit.position;

                                newHandler = agent.GetComponent<KinematicEntity>().MoveGoal;

                                newGoal = new Goal(goalActionTime: Random.Range(1, 10), targetPositions: new List<Vector3> { targetPosition }, interactionObject: null);
                                GroupManagerInstance.SetGroupAction(groupID, Utilities.Actions.Nothing, newGoal);

                                groupMembers = GroupManagerInstance.GetGroupAgents(groupID);

                                for (int i = 0; i < groupMembers.Count; ++i)
                                {
                                    memberKinematic = groupMembers[i].GetComponent<KinematicEntity>();
                                    GoalHandler tmpHandler = memberKinematic.MoveGoal;
                                    Goal tmpGoal = new Goal(newGoal, tmpHandler);
                                    groupMembers[i].GetComponent<KinematicEntity>().SetNewGoal(tmpGoal);
                                }

                                newHandler = agent.GetComponent<KinematicEntity>().MoveGoal;

                                newGoal = new Goal(newGoal, goalFunction: newHandler);
                                Debug.Log("Group Move");

                                return newGoal;

                            default:
                                throw new System.Exception("Invalid goal requested");
                        }
                    }
                case Utilities.Actions.Wait:
                    {
                        newHandler = agent.GetComponent<KinematicEntity>().WaitGoal;

                        newGoal = new Goal(goalFunction: newHandler, goalActionTime: 100f, targetPositions: null, interactionObject: null);

                        GroupManagerInstance.SetGroupAction(groupID, Utilities.Actions.Nothing, newGoal);

                        groupMembers = GroupManagerInstance.GetGroupAgents(groupID);

                        for (int i = 0; i < groupMembers.Count; ++i)
                        {
                            memberKinematic = groupMembers[i].GetComponent<KinematicEntity>();
                            GoalHandler tmpHandler = memberKinematic.InteractionGoal;
                            Goal tmpGoal = new Goal(newGoal, tmpHandler);
                            groupMembers[i].GetComponent<KinematicEntity>().SetNewGoal(tmpGoal);
                        }

                        newHandler = agent.GetComponent<KinematicEntity>().InteractionGoal;

                        newGoal = new Goal(newGoal, goalFunction: newHandler);

                        Debug.Log("Group Wait");
                        return newGoal;
                    }

                default:
                    throw new System.Exception("Unexpected group previous action");
            }
        }
        else
        {
            actionType = BehaviourProbabilities.GetBehaviourType(agentJob);

            if (Target != null)
            {
                HotspotManagerInstance.UpdateHotSpot(Target.name, 1);
            }

            switch (actionType)
            {
                case Utilities.Actions.Interact:

                    GameObject hotspot = HotspotManagerInstance.getRandomHotSpot();
                    if (hotspot == null)
                    {
                        goto case Utilities.Actions.Move;
                    }

                    Debug.Log("Interact");

                    newHandler = agent.GetComponent<KinematicEntity>().InteractionGoal;

                    newGoal = new Goal(goalFunction: newHandler, goalActionTime: 3f, targetPositions: new List<Vector3> { hotspot.transform.position }, interactionObject: new List<GameObject> { hotspot });
                    return newGoal;

                        
                case Utilities.Actions.Patrol:

                    List<Vector3> patrolRoute = PatrolManagerInstance.GetPatrolRoute();
                    if (patrolRoute == null)
                    {
                        goto case Utilities.Actions.Move;
                    }

                    newHandler = agent.GetComponent<KinematicEntity>().PatrolGoal;

                    newGoal = new Goal(goalFunction: newHandler, goalActionTime: 0.5f, targetPositions: patrolRoute, interactionObject: null);
                    return newGoal;

                case Utilities.Actions.Move:

                    Vector3 targetPosition = new Vector3();
                    targetPosition.x = Random.Range(LeftBound.x, RightBound.x);
                    targetPosition.z = Random.Range(LeftBound.z, RightBound.z);

                    Debug.Log("Move");

                    newHandler = agent.GetComponent<KinematicEntity>().MoveGoal;

                    newGoal = new Goal(goalFunction: newHandler, goalActionTime: 3f, targetPositions: new List<Vector3> { targetPosition }, interactionObject: null);
                    return newGoal;

                case Utilities.Actions.Exit:

                    newGoal = UpdateEmergency(agent);
                    return newGoal;

                default:
                    Debug.Log(actionType);
                    throw new System.Exception("Invalid goal requested");
            }
        }

    }


    public Goal UpdateEmergency(GameObject agent)
    {
        Goal newGoal;
        GoalHandler newHandler;
        KinematicEntity agentKinematic = agent.GetComponent<KinematicEntity>();
        EmergencyLocator agentEmergencyLocator = agent.GetComponent<EmergencyLocator>();

        List<GameObject> exitObject;
        List<Vector3> exitPosition;

        newHandler = agentKinematic.ExitGoal;
        agentEmergencyLocator.GetExit(out exitPosition,out exitObject);

        newGoal = new Goal(goalFunction: newHandler, goalActionTime: 0f, targetPositions: exitPosition, interactionObject: exitObject);

        agentKinematic.SetNewGoal(newGoal);

        return newGoal;
    }

    

    public void DeclareEmergencyState()
    {
        Goal newGoal;
        GoalHandler newHandler;
        KinematicEntity agentKinematic;
        EmergencyLocator agentEmergencyLocator;
        List<GameObject> exitObject;
        List<Vector3> exitPosition;

        for (int i = 0; i < Agents.Count; ++i)
        {
            agentKinematic = Agents[i].GetComponent<KinematicEntity>();
            agentEmergencyLocator = Agents[i].GetComponent<EmergencyLocator>();
            newHandler = agentKinematic.ExitGoal;
            agentEmergencyLocator.GetExit(out exitPosition, out exitObject);

            newGoal = new Goal(goalFunction: newHandler, goalActionTime: 0f, targetPositions: exitPosition, interactionObject: exitObject);
            agentKinematic.SetNewGoal(newGoal);
            agentKinematic.ToggleTrailRendering();
        }
    }


    public void AssignAgentsList(List<GameObject> agents)
    {
        this.Agents = agents;
    }

    public void SetSpawnPointsFlagOn()
    {
        HotspotsReady = true;
    }
    public bool IsReady()
    {
        return HotspotsReady;
    }


    
   
}
