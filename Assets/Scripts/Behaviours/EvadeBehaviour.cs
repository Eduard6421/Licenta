using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeBehaviour : ISteerable
{

    private CharacterBehaviour FleeingBehaviour;

    private float MaxPrediction;
    private float MaxAcceleration;
    private SteeringOutput newSteering;

    public EvadeBehaviour(float maxAcceleration, float maxSpeed, float maxAngularAcceleration, float maxRotation, float slowRadius, float targetRadius, float timeToTarget, float maxPrediction) 
    {

        this.MaxAcceleration = maxAcceleration;
        this.MaxPrediction   = maxPrediction;
        this.newSteering = new SteeringOutput();

        FleeingBehaviour = new FleeBehaviour(maxAcceleration);

    }


    public SteeringOutput GetSteering(Vector3 characterPosition, float characterOrientation, Vector3 currentVelocity, float currentRotation, Vector3 targetPosition, float targetOrientation, Vector3 targetVelocity, float targetRotation)
    {

        newSteering.linearAcceleration = Vector3.zero;
        newSteering.angularAcceleration = 0;

        Vector3 newTargetPosition;
        Vector3 TargetDirection;

        float distance;
        float speedTotal;
        float predictionTime;

        TargetDirection = targetPosition - characterPosition;
        distance = TargetDirection.magnitude;

        speedTotal = currentVelocity.magnitude;

        if (speedTotal <= distance / MaxPrediction)
        {
            predictionTime = MaxPrediction;
        }
        else
        {
            predictionTime = distance / speedTotal;
        }

        newTargetPosition = targetPosition + targetVelocity * predictionTime;

        return FleeingBehaviour.GetSteering(characterPosition, characterOrientation, currentVelocity, currentRotation, newTargetPosition, targetOrientation, targetVelocity, targetRotation);

    }





}
