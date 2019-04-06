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
        SteeringOutput steering = new SteeringOutput();

        steering.linearSpeed = characterPosition - targetPosition;
        steering.linearSpeed.Normalize();
        steering.linearSpeed = steering.linearSpeed * MaxAcceleration;

        return steering;
    }

}
