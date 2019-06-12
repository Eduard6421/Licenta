using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LeaveBehaviour : CharacterBehaviour
{

    public LeaveBehaviour(float maxAcceleration, float maxSpeed, float maxAngularAcceleration, float maxRotation, float slowRadius, float targetRadius, float timeToTarget) : base(maxAcceleration, maxSpeed, maxAngularAcceleration, maxRotation, slowRadius, targetRadius, timeToTarget)
    {

    }

    public override SteeringOutput GetSteering(Vector3 characterPosition, float characterOrientation, Vector3 currentVelocity, float currentRotation, Vector3 targetPosition, float targetOrientation, Vector3 targetVelocity, float targetRotation)
    {
        newSteering.linearAcceleration = Vector3.zero;
        newSteering.angularAcceleration = 0;

        Vector3 DesiredDirection;
        Vector3 DesiredVelocity;

        float distanceToTarget;
        float targetSpeed;

        DesiredDirection = characterPosition - targetPosition;
        distanceToTarget = DesiredDirection.magnitude;

        if (distanceToTarget > TargetRadius)
        {
            return newSteering;
        }

        if (distanceToTarget < SlowRadius)
        {
            targetSpeed = MaxSpeed;
        }
        else
        {
            targetSpeed = MaxSpeed * (TargetRadius - distanceToTarget) / SlowRadius;
        }

        DesiredVelocity = DesiredDirection;
        DesiredVelocity.Normalize();
        DesiredVelocity = DesiredVelocity * targetSpeed;

        newSteering.linearAcceleration = DesiredVelocity - currentVelocity;
        newSteering.linearAcceleration /= TimeToTarget;

        if (newSteering.linearAcceleration.magnitude > MaxAcceleration)
        {
            newSteering.linearAcceleration.Normalize();
            newSteering.linearAcceleration *= MaxAcceleration;
        }

        newSteering.linearAcceleration = newSteering.linearAcceleration * targetSpeed;


        return newSteering;

    }

}
