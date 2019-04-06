using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityMatchBehaviour : CharacterBehaviour
{
    public VelocityMatchBehaviour(float maxAcceleration, float maxSpeed, float maxAngularAcceleration, float maxRotation, float slowRadius, float targetRadius, float timeToTarget) : base(maxAcceleration, maxSpeed, maxAngularAcceleration, maxRotation, slowRadius, targetRadius, timeToTarget)
    {

    }

    public override SteeringOutput GetSteering(Vector3 characterPosition, float characterOrientation, Vector3 currentVelocity, float currentRotation, Vector3 targetPosition, float targetOrientation, Vector3 targetVelocity, float targetRotation)
    {

        SteeringOutput newSteering = new SteeringOutput();

        Vector3 DesiredVelocity;
        
        DesiredVelocity = targetVelocity - currentVelocity;
        DesiredVelocity /= TimeToTarget;


        if( DesiredVelocity.magnitude > MaxAcceleration)
        {
            DesiredVelocity.Normalize();
            DesiredVelocity *= MaxAcceleration;
        }


        newSteering.linearSpeed = DesiredVelocity;
        newSteering.angularSpeed = 0;

        return newSteering;

    }
}
