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

    private static float patrolProbability;

    private static List<Utilities.Jobs> jobList = new List<Utilities.Jobs> { Utilities.Jobs.Civilian, Utilities.Jobs.Patrolman, Utilities.Jobs.GroupMember };
    private static List<float> probabilities = new List<float> { 0.5f, 0f, 1f };


    public static void StopGroupDistributon()
    {
        probabilities[0] = probabilities[0] + probabilities[2] / 2;
        probabilities[1] = probabilities[1] + probabilities[2] / 2;
        probabilities[2] = 0;
    }


    public static Utilities.Jobs GetAgentType()
    {
        float random = Random.Range(0, 1f);
        

        for(int i = 0; i < probabilities.Count; ++i)
        {
            if(random <= probabilities[i])
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

                interactProbability = 0f;
                moveProbability = 1f;
                exitProbability = 0f;

                random = Random.Range(0, 1f);

                if (random <= interactProbability)
                    return Utilities.Actions.Interact;
                if (random <= moveProbability)
                    return Utilities.Actions.Move;
                return Utilities.Actions.Exit;



            case Utilities.Jobs.Patrolman:
                interactProbability = 0.0f;
                patrolProbability = 1f;
                exitProbability = 0f;

                random = Random.Range(0, 1f);

                if (random <= interactProbability)
                    return Utilities.Actions.Interact;
                if (random <= patrolProbability)
                    return Utilities.Actions.Patrol;
                if (random <= moveProbability)
                    return Utilities.Actions.Move;
                return Utilities.Actions.Exit;

            case Utilities.Jobs.GroupMember:
                interactProbability = 1f;
                moveProbability = 0;
                exitProbability = 0f;

                random = Random.Range(0, 1f);

                if (random <= interactProbability)
                    return Utilities.Actions.Interact;
                if (random <= moveProbability)
                    return Utilities.Actions.Move;
                return Utilities.Actions.Exit;

            default:
                return Utilities.Actions.Exit;
        }

    }



}
