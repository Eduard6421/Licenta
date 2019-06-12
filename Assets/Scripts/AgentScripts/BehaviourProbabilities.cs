using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BehaviourProbabilities
{

    /* The sum of probabilities must be equal to 1 */

    private static float moveProbability;
    private static float interactProbability;
    private static float meetProbabilty;
    private static float exitProbability;
    private static float total;
    private static bool noGroups = false;

    private static float patrolProbability;

    private static List<Utilities.Jobs> jobList = new List<Utilities.Jobs> { Utilities.Jobs.Civilian, Utilities.Jobs.GroupMember, Utilities.Jobs.Patrolman};
    private static List<float> groupProbabilities = new List<float> { 0.5f, 0.4f, 0.1f };


    public static void StopGroupDistributon()
    {
        groupProbabilities[0] = groupProbabilities[0] + groupProbabilities[1] / 2;
        groupProbabilities[2] = groupProbabilities[2] + groupProbabilities[1] / 2;
        groupProbabilities[1] = 0;
        noGroups = true;
    }


    public static Utilities.Jobs GetAgentType()
    {
        float random = Random.Range(0, 1f);
        

        for(int i = 0; i < groupProbabilities.Count; ++i)
        {
            float sum = 0;

            for (int j = 0; j <= i; ++j)
            {
                sum += groupProbabilities[j];
            }

            if(random <= sum)
            {

                return jobList[i];
            }
        }

        return jobList[0];

    }

    public static Utilities.Actions GetBehaviourType(Utilities.Jobs AgentType)
    {
        float random;

        switch (AgentType)
        {
            case Utilities.Jobs.Civilian:

                interactProbability = 0.85f;
                moveProbability = 0.200f;
                exitProbability = 0.005f;

                random = Random.Range(0, 1f);
                
                if (random <= interactProbability)
                    return Utilities.Actions.Interact;
                if (random <= interactProbability + moveProbability)
                    return Utilities.Actions.Move;

                return Utilities.Actions.Exit;



            case Utilities.Jobs.Patrolman:
                interactProbability = 0.2f;
                patrolProbability = 0.8f;
                exitProbability = 0f;

                random = Random.Range(0, 1f);

                if (random <= interactProbability)
                    return Utilities.Actions.Interact;
                if (random <= interactProbability+ patrolProbability)
                    return Utilities.Actions.Patrol;
                if (random <= interactProbability + moveProbability + patrolProbability)
                    return Utilities.Actions.Move;
                return Utilities.Actions.Exit;

            case Utilities.Jobs.GroupMember:

                interactProbability = 0.85f;
                moveProbability = 0.200f;
                exitProbability = 0.00f;

                random = Random.Range(0, 1f);

                if (random <= interactProbability)
                    return Utilities.Actions.Interact;
                if (random <= moveProbability + interactProbability)
                    return Utilities.Actions.Move;
                return Utilities.Actions.Exit;

            default:
                return Utilities.Actions.Exit;
        }

    }



}
