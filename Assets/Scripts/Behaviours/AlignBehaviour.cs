using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignBehaviour : CharacterBehaviour
{

    public AlignBehaviour(float maxAcceleration, float maxSpeed, float maxAngularAcceleration, float maxRotation, float slowRadius, float targetRadius, float timeToTarget) : base(maxAcceleration, maxSpeed, maxAngularAcceleration, maxRotation, slowRadius, targetRadius, timeToTarget)
    {

    }

    public override SteeringOutput GetSteering(Vector3 characterPosition, float characterOrientation, Vector3 currentVelocity, float currentRotation, Vector3 targetPosition, float targetOrientation, Vector3 targetVelocity, float targetRotation)
    {
        newSteering.angularAcceleration = 0;
        newSteering.linearAcceleration = Vector3.zero;
        
        float rotation;
        float rotationLength;

        float targetRotationSpeed;
        float angularAcceleration;


        rotation = Utilities.getAngleDifference(targetOrientation, characterOrientation);
        rotationLength = Mathf.Abs(rotation);

        if (rotationLength < TargetRadius)
        {
            return newSteering;
        }

        else if (rotationLength > SlowRadius)
        {
            targetRotationSpeed = MaxRotation;
        }
        else
        {
            targetRotationSpeed = MaxRotation * rotationLength / SlowRadius;
        }

        targetRotationSpeed *= rotation / rotationLength;

        newSteering.angularAcceleration = targetRotationSpeed - currentRotation;
        newSteering.angularAcceleration /= TimeToTarget;

        angularAcceleration = Mathf.Abs(newSteering.angularAcceleration);

        if(angularAcceleration > MaxAngularAcceleration)
        {
            newSteering.angularAcceleration /= angularAcceleration;
            newSteering.angularAcceleration *= MaxAngularAcceleration;
        }

        return newSteering;

    }


}
