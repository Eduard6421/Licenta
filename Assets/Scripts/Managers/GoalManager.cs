using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    private static GoalManager instance = null;

    private List<GameObject> Agents;

    private HotSpotManager HotspotManagerInstance;

    private PatrolManager PatrolManagerInstance;

    private static bool HotspotsReady = false;

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
    }


    public Goal RequestBehaviour(GameObject agent, Utilities.Jobs agentJob, int groupID)
    {

        Goal newGoal;
        GoalHandler newHandler;

        if (agentJob == Utilities.Jobs.GroupMember)
        {


            return null;


        }
        else
        {

            string behaviourType = BehaviourProbabilities.GetBehaviourType(agentJob);

            switch (behaviourType)
            {
                case "Interact":

                    GameObject hotspot = HotspotManagerInstance.getRandomHotSpot();
                    if (hotspot == null)
                    {
                        goto case "Move";
                    }

                    newHandler = agent.GetComponent<KinematicEntity>().InteractionGoal;

                    newGoal = new Goal(goalFunction: newHandler, goalActionTime: 3f, targetPosition: new List<Vector3> { hotspot.transform.position }, interactionObject: new List<GameObject> { hotspot });
                    return newGoal;


                case "Patrol":

                    List<Vector3> patrolRoute = PatrolManagerInstance.GetPatrolRoute();
                    if (patrolRoute == null)
                    {
                        goto case "Move";
                    }

                    newHandler = agent.GetComponent<KinematicEntity>().PatrolGoal;

                    newGoal = new Goal(goalFunction: newHandler, goalActionTime: 0.5f, targetPosition: patrolRoute, interactionObject: null);
                    return newGoal;

                case "Move":



                    break;

                case "Meet":
                    break;

            }

            return null;

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

    public static bool IsReady()
    {
        Debug.Log("fuck yeah" + HotspotsReady);
        return HotspotsReady;
    }

    
   
}
