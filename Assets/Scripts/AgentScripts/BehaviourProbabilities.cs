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

    public static string GetBehaviourType(Utilities.Jobs AgentType)
    {
        float random;

        switch (AgentType)
        {
            case Utilities.Jobs.Civilian:
                interactProbability = 1f;
                moveProbability = 0f;
                meetProbabilty = 0f;
                exitProbability = 0f;

                total = interactProbability + moveProbability+ meetProbabilty + exitProbability;

                random = Random.Range(0, 1f);

                if (random <= interactProbability)
                    return "Interact";
                if (random <= moveProbability)
                    return "Move";
                if (random <= meetProbabilty)
                    return "Meet";
                return "Exit";



            case Utilities.Jobs.Patrolman:
                interactProbability = 0.0f;
                patrolProbability = 1f;

                total = interactProbability + patrolProbability + moveProbability + meetProbabilty + exitProbability;

                random = Random.Range(0, 1f);

                if (random <= interactProbability)
                    return "Interact";
                if (random <= patrolProbability)
                    return "Patrol";
                if (random <= moveProbability)
                    return "Move";
                if (random <= meetProbabilty)
                    return "Meet";
                return "Exit";


            default:
                return "Exit";
        }

    }



}
