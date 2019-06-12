using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianBuilder : BehaviourBuilder
{
    public  BlendedSteeringBehaviour walkingSteering(List<Vector3> cornerArray, List<GameObject> agents)
    {

        float maxSpeed = 5f;
        float maxRotation = 5f;

        ISteerable pathFollowing = BehaviourFactory.GetCharacterBehaviour(Utilities.Behaviour.PathFollowing, maxSpeed,
            maxRotation, cornerArray, agents);

        ISteerable obstacleAvoidance = BehaviourFactory.GetCharacterBehaviour(Utilities.Behaviour.ObstacleAvoidance,
            maxSpeed, maxRotation, cornerArray, agents);

        ISteerable wanderBehavour = BehaviourFactory.GetCharacterBehaviour(Utilities.Behaviour.Wander,
            maxSpeed, maxRotation, cornerArray, agents);

        WeightedBehaviour pathFollow = new WeightedBehaviour(pathFollowing,0.5f);    
        WeightedBehaviour collisionAvoidance  = new WeightedBehaviour(obstacleAvoidance,0.2f);
        WeightedBehaviour wanderBehaviour = new WeightedBehaviour(wanderBehavour, 0.3f);

        List<WeightedBehaviour> behaviourList = new List<WeightedBehaviour>();

        behaviourList.Add(pathFollow);
        behaviourList.Add(collisionAvoidance);
        behaviourList.Add(wanderBehaviour);


        return new BlendedSteeringBehaviour(maxSpeed,maxRotation,behaviourList);
    }

    
    public  BlendedSteeringBehaviour runningSteering(List<Vector3> cornerArray, List<GameObject> agents)
    {
        return null;
    }
    

}

