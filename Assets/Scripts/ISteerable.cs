using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISteerable
{
    SteeringOutput GetSteering(Vector3 characterPosition, float characterOrientation, Vector3 currentVelocity, float currentRotation, Vector3 targetPosition, float targetOrientation, Vector3 targetVelocity, float targetRotation);
}

