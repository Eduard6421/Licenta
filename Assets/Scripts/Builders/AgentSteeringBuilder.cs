using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BehaviourBuilder
{
      BlendedSteeringBehaviour walkingSteering(List<Vector3> cornerArray, List<GameObject> agents);
      BlendedSteeringBehaviour runningSteering(List<Vector3> cornerArray, List<GameObject> agents);
}
