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
    private SteeringOutput newSteering;
    private ISteerable FaceBehaviour;

    public WanderBehaviour(float maxAcceleration, float maxSpeed, float maxAngularAcceleration, float maxRotation, float slowRadius, float targetRadius, float timeToTarget, float WanderOffset, float WanderRadius, float WanderRate, float WanderOrientation)
    {
        this.WanderOffset = WanderOffset;
        this.WanderRadius = WanderRadius;
        this.WanderRate = WanderRate;
        this.WanderOrientation = WanderOrientation;
        this.MaxAcceleration = maxAcceleration;
        this.newSteering = new SteeringOutput();

        FaceBehaviour = new FaceBehaviour(maxAcceleration, maxSpeed, maxAngularAcceleration, maxRotation, slowRadius, targetRadius, timeToTarget);

    }


    public SteeringOutput GetSteering(Vector3 characterPosition, float characterOrientation, Vector3 currentVelocity, float currentRotation, Vector3 targetPosition, float targetOrientation, Vector3 targetVelocity, float targetRotation)
    {
        Vector3 characterDirectionalVector;
        Vector3 targetDirectionalVector;

        newSteering.linearAcceleration = Vector3.zero;
        newSteering.angularAcceleration = 0;


        WanderOrientation = Utilities.randomBinomial() * WanderRate;

        targetOrientation = characterOrientation + WanderOrientation;

        characterDirectionalVector = Utilities.getDirectionalVector(characterOrientation);

        targetPosition = characterPosition + WanderOffset * characterDirectionalVector;

        targetDirectionalVector = Utilities.getDirectionalVector(targetOrientation);

        targetPosition += WanderRadius * targetDirectionalVector;

        newSteering = FaceBehaviour.GetSteering(characterPosition, characterOrientation, currentVelocity, currentRotation, targetPosition, targetOrientation, targetVelocity, targetRotation);

        newSteering.linearAcceleration = MaxAcceleration * characterDirectionalVector;

        return newSteering;

    }


}
