using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderBehaviour : ISteerable
{
    private float WanderOffset;
    private float WanderRadius;
    private float WanderRate;
    private float WanderOrientation;
    private float MaxAcceleration;

    private ISteerable FaceBehaviour;

    public WanderBehaviour(float maxAcceleration, float maxSpeed, float maxAngularAcceleration, float maxRotation, float slowRadius, float targetRadius, float timeToTarget, float WanderOffset, float WanderRadius, float WanderRate, float WanderOrientation)
    {
        this.WanderOffset = WanderOffset;
        this.WanderRadius = WanderRadius;
        this.WanderRate = WanderRate;
        this.WanderOrientation = WanderOrientation;
        this.MaxAcceleration = maxAcceleration;

        maxAcceleration = 3f;
        maxRotation = 5f;
        slowRadius = 10f;
        targetRadius = 1f;
        timeToTarget = 1f;

        FaceBehaviour = new FaceBehaviour(maxAcceleration, maxSpeed, maxAngularAcceleration, maxRotation, slowRadius, targetRadius, timeToTarget);

    }


    public SteeringOutput GetSteering(Vector3 characterPosition, float characterOrientation, Vector3 currentVelocity, float currentRotation, Vector3 targetPosition, float targetOrientation, Vector3 targetVelocity, float targetRotation)
    {
        SteeringOutput newSteering = new SteeringOutput();

        Vector3 characterDirectionalVector;
        Vector3 targetDirectionalVector;

        WanderOrientation = Utilities.randomBinomial() * WanderRate;

        targetOrientation = characterOrientation + WanderOrientation;

        characterDirectionalVector = Utilities.getDirectionalVector(characterOrientation);

        targetPosition = characterPosition + WanderOffset * characterDirectionalVector;

        targetDirectionalVector = Utilities.getDirectionalVector(targetOrientation);

        targetPosition += WanderRadius * targetDirectionalVector;

        newSteering = FaceBehaviour.GetSteering(characterPosition, characterOrientation, currentVelocity, currentRotation, targetPosition, targetOrientation, targetVelocity, targetRotation);

        newSteering.linearSpeed = MaxAcceleration * characterDirectionalVector;

        return newSteering;

    }


}
