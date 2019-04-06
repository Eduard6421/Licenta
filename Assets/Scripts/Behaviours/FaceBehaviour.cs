using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceBehaviour : ISteerable
{
    private CharacterBehaviour AlignBehaviour;


    public FaceBehaviour(float maxAcceleration, float maxSpeed, float maxAngularAcceleration, float maxRotation, float slowRadius, float targetRadius, float timeToTarget) 
    {
        AlignBehaviour = new AlignBehaviour(maxAcceleration, maxSpeed, maxAngularAcceleration, maxRotation, slowRadius, targetRadius, timeToTarget);
    }


    public SteeringOutput GetSteering(Vector3 characterPosition, float characterOrientation, Vector3 currentVelocity, float currentRotation, Vector3 targetPosition, float targetOrientation, Vector3 targetVelocity, float targetRotation)
    {
        SteeringOutput newSteering = new SteeringOutput();

        Vector3 targetDirection;

        targetDirection = targetPosition - characterPosition;

        if (targetDirection.magnitude == 0)
        {
            return newSteering;
        }

        targetOrientation = Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg; ;

        newSteering = AlignBehaviour.GetSteering(characterPosition, characterOrientation, currentVelocity, currentRotation, targetPosition, targetOrientation, targetVelocity, targetRotation);

        return newSteering;


    }

}
