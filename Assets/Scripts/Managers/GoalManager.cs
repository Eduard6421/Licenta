using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    private static GoalManager instance = null;

    private List<GameObject> Agents;

    private HotSpotManager HotspotManagerInstance;

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
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        HotspotManagerInstance = HotSpotManager.GetInstance();
    }


    public Goal RequestBehaviour(GameObject agent, string agentJob)
    {

        Goal newGoal;
        string behaviourType = BehaviourProbabilities.GetBehaviourType(agentJob);

        switch (behaviourType)
        {
            case "Interact":

                GameObject hotspot = HotspotManagerInstance.getRandomHotSpot();
                if(hotspot == null)
                {
                    goto case "Move";
                }

                Goal.GoalHandler newHandler = agent.GetComponent<KinematicEntity>().InteractionGoal;

                newGoal = new Goal(newHandler, 3f, hotspot.transform.position, hotspot);
                return newGoal;

            case "Move":
                    
                

                break;

            case "Meet":
                break;

        }

        return null;

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
