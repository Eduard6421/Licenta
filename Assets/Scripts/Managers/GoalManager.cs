using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            // The group is doing nothing currently

            switch (actionType)
            {

                case Utilities.Actions.Nothing:
                    {
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


                                newGoal = new Goal(goalActionTime: 3f, targetPositions: new List<Vector3> { hotspot.transform.position }, interactionObject: new List<GameObject> { hotspot });
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
                                return newGoal;

                            case Utilities.Actions.Move:
                                return null;

                            default:
                                return null;
                        }
                    }
                case Utilities.Actions.Wait:
                    {
                        newHandler = agent.GetComponent<KinematicEntity>().WaitGoal;

                        newGoal = new Goal(goalFunction: newHandler, goalActionTime: 100f, targetPositions: null, interactionObject: null);
                        return newGoal;
                    }

                default:
                    return null;
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

                    newHandler = agent.GetComponent<KinematicEntity>().MoveGoal;

                    newGoal = new Goal(goalFunction: newHandler, goalActionTime: 3f, targetPositions: new List<Vector3> { targetPosition }, interactionObject: null);
                    return newGoal;

                default:
                    throw new System.Exception("Invalid goal requested");
            }
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
