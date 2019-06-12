using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAndRunBehaviour : ISteerable
{
    ISteerable FaceBehaviour;
    ISteerable ArriveBehaviour;
    private SteeringOutput newSteering;

    public LookAndRunBehaviour(float maxAcceleration, float maxSpeed, float maxAngularAcceleration, float maxRotation, float slowRadius, float targetRadius, float timeToTarget)
    {
        this.newSteering = new SteeringOutput();

        ArriveBehaviour = new ArriveBehaviour(maxAcceleration, maxSpeed, maxAngularAcceleration, maxRotation, slowRadius, targetRadius, timeToTarget);
        FaceBehaviour = new FaceBehaviour(maxAcceleration, maxSpeed, maxAngularAcceleration, maxRotation, slowRadius, targetRadius, timeToTarget);
    }

    public  SteeringOutput GetSteering(Vector3 characterPosition, float characterOrientation, Vector3 currentVelocity, float currentRotation, Vector3 targetPosition, float targetOrientation, Vector3 targetVelocity, float targetRotation)
    {
        newSteering.linearAcceleration = Vector3.zero;
        newSteering.angularAcceleration = 0;

        newSteering = ArriveBehaviour.GetSteering(characterPosition, characterOrientation, currentVelocity, currentRotation, targetPosition, targetOrientation, targetVelocity, targetRotation);
        newSteering.angularAcceleration = FaceBehaviour.GetSteering(characterPosition, characterOrientation, currentVelocity, currentRotation, characterPosition + currentVelocity, targetOrientation, targetVelocity, targetRotation).angularAcceleration;

        return newSteering;
    }

}
