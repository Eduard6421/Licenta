using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueBehaviour : ISteerable
{

    private SeekBehaviour SeekingBehaviour;
    private float MaxPrediction;
    private float MaxAcceleration;


    public PursueBehaviour(float maxAcceleration, float maxSpeed, float maxAngularAcceleration, float maxRotation, float slowRadius, float targetRadius, float timeToTarget, float maxPrediction)
    {

        MaxAcceleration = maxAcceleration;
        SeekingBehaviour = new SeekBehaviour(maxAcceleration);
        MaxPrediction = maxPrediction;
    }


    public SteeringOutput GetSteering(Vector3 characterPosition, float characterOrientation, Vector3 currentVelocity, float currentRotation, Vector3 targetPosition, float targetOrientation, Vector3 targetVelocity, float targetRotation)
    {

        Vector3 newTargetPosition;
        Vector3 TargetDirection;

        float distance;
        float speedTotal;
        float predictionTime;

        TargetDirection = targetPosition - characterPosition;
        distance = TargetDirection.magnitude;

        speedTotal = currentVelocity.magnitude;
        
        if(speedTotal <= distance/MaxPrediction)
        {
            predictionTime = MaxPrediction;
        }
        else
        {
            predictionTime = distance / speedTotal;
        }

        newTargetPosition = targetPosition + targetVelocity * predictionTime;

        return SeekingBehaviour.GetSteering(characterPosition, characterOrientation, currentVelocity, currentRotation, newTargetPosition, targetOrientation, targetVelocity, targetRotation);

    }





}
