using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviourBuilder
{
    public abstract SteeringOutput getAvoidanceGroupSteering();
    public abstract SteeringOutput getMovementGroupSteering();
}
