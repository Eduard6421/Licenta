using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviourBuilder
{
    public abstract BlendedSteeringBehaviour walkingSteering(float maxSpeed, float maxRotation, List<Vector3> cornerArray, List<GameObject> agents);
    public abstract BlendedSteeringBehaviour runningSteering(float maxSpeed, float maxRotation, List<Vector3> cornerArray, List<GameObject> agents);



}
