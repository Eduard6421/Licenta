using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeBehaviour : CharacterBehaviour
{

    public FleeBehaviour(float maxAcceleration) : base(maxAcceleration)
    {

    }


    public override SteeringOutput GetSteering(Vector3 characterPosition, float characterOrientation, Vector3 currentVelocity, float currentRotation, Vector3 targetPosition, float targetOrientation, Vector3 targetVelocity, float targetRotation)
    {

        newSteering.linearAcceleration = Vector3.zero;
        newSteering.angularAcceleration = 0;

        newSteering.linearAcceleration = characterPosition - targetPosition;
        newSteering.linearAcceleration.Normalize();
        newSteering.linearAcceleration = newSteering.linearAcceleration * MaxAcceleration;

        return newSteering;
    }

}
