using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianBuilder : BehaviourBuilder
{

    public CivilianBuilder()
    {
    }

    public override BlendedSteeringBehaviour walkingSteering(float maxSpeed, float maxRotation, List<Vector3> cornerArray, List<GameObject> agents)
    {
        ISteerable pathFollowing = BehaviourFactory.GetCharacterBehaviour(Utilities.Behaviour.PathFollowing, maxSpeed,
            maxRotation, cornerArray, agents);

        ISteerable obstacleAvoidance = BehaviourFactory.GetCharacterBehaviour(Utilities.Behaviour.ObstacleAvoidance,
            maxSpeed, maxRotation, cornerArray, agents);

        WeightedBehaviour path_follow = new WeightedBehaviour(pathFollowing,0.6f);    
        WeightedBehaviour collision_avoidance  = new WeightedBehaviour(obstacleAvoidance,0.1f);

        List<WeightedBehaviour> behaviourList = new List<WeightedBehaviour>();

        behaviourList.Add(path_follow);
        behaviourList.Add(collision_avoidance);


        return new BlendedSteeringBehaviour(maxSpeed,maxRotation,behaviourList);
    }

    
    public override BlendedSteeringBehaviour runningSteering(float maxSpeed, float maxRotation, List<Vector3> cornerArray, List<GameObject> agents)
    {
        return null;
    }
    

}

