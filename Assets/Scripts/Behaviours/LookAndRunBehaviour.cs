using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAndRunBehaviour : ISteerable
{
    ISteerable FaceBehaviour;
    ISteerable ArriveBehaviour;

    public LookAndRunBehaviour(float maxAcceleration, float maxSpeed, float maxAngularAcceleration, float maxRotation, float slowRadius, float targetRadius, float timeToTarget)
    {

        ArriveBehaviour = new ArriveBehaviour(maxAcceleration, maxSpeed, maxAngularAcceleration, maxRotation, slowRadius, targetRadius, timeToTarget);

        maxAcceleration = 3f;
        maxRotation = 5f;
        slowRadius = 10f;
        targetRadius = 1f;
        timeToTarget = 1f;

        FaceBehaviour = new FaceBehaviour(maxAcceleration, maxSpeed, maxAngularAcceleration, maxRotation, slowRadius, targetRadius, timeToTarget);

    }

    public  SteeringOutput GetSteering(Vector3 characterPosition, float characterOrientation, Vector3 currentVelocity, float currentRotation, Vector3 targetPosition, float targetOrientation, Vector3 targetVelocity, float targetRotation)
    {
        SteeringOutput newSteering;

        newSteering = ArriveBehaviour.GetSteering(characterPosition, characterOrientation, currentVelocity, currentRotation, targetPosition, targetOrientation, targetVelocity, targetRotation);
        

        if (currentVelocity.magnitude != 0)
        {
            newSteering.angularSpeed = FaceBehaviour.GetSteering(characterPosition, characterOrientation, currentVelocity, currentRotation, characterPosition + currentVelocity, targetOrientation, targetVelocity, targetRotation).angularSpeed;
        }


        return newSteering;
    }

}
