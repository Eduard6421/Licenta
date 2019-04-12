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

    public static string GetBehaviourType(Utilities.Jobs AgentType)
    {


        switch (AgentType)
        {
            case Utilities.Jobs.Civilian:
                interactProbability = 1f;
                moveProbability = 0f;
                meetProbabilty = 0f;
                exitProbability = 0f;
                break;

            default:
                break;
        }

        total = moveProbability + interactProbability + meetProbabilty + exitProbability;

        float random = Random.Range(0, 1f);

        if(random <= interactProbability)
            return "Interact";
        if (random <= moveProbability)
            return "Move";
        if (random <= meetProbabilty)
            return "Meet";
        
        return "Exit";
    }



}
