using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignOppositeBehaviour : CharacterBehaviour
{

    public AlignOppositeBehaviour(float maxAcceleration, float maxSpeed, float maxAngularAcceleration, float maxRotation, float slowRadius, float targetRadius, float timeToTarget) : base(maxAcceleration, maxSpeed, maxAngularAcceleration, maxRotation, slowRadius, targetRadius, timeToTarget)
    {

    }

    public override SteeringOutput GetSteering(Vector3 characterPosition,float characterOrientation, Vector3 currentVelocity, float currentRotation,  Vector3 targetPosition,float targetOrientation, Vector3 targetVelocity, float targetRotation)
    {
        SteeringOutput newSteering = new SteeringOutput();

        float rotation;
        float rotationLength;

        float targetRotationSpeed;
        float angularAcceleration;


        rotation = Utilities.getAngleDifference(targetOrientation + 180, characterOrientation);
        rotationLength = Mathf.Abs(rotation);


        if (rotationLength < TargetRadius)
        {
            return null;
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

        newSteering.angularSpeed = targetRotationSpeed - currentRotation;
        newSteering.angularSpeed /= TimeToTarget;

        angularAcceleration = Mathf.Abs(newSteering.angularSpeed);

        if (angularAcceleration > MaxAngularAcceleration)
        {
            newSteering.angularSpeed /= angularAcceleration;
            newSteering.angularSpeed *= MaxAngularAcceleration;
        }

        return newSteering;

    }


}
