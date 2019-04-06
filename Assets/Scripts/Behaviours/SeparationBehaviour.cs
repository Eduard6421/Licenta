using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparationBehaviour : ISteerable
{

    private List<GameObject> Targets;

    private float MaxAcceleration;
    private float DecayCoefficient;
    private float TargetRadius;

    public SeparationBehaviour(float maxAcceleration, float maxSpeed, float maxAngularAcceleration, float maxRotation, float slowRadius, float targetRadius, float timeToTarget, List<GameObject> targets, float decayCoefficent) 
    {
        this.Targets = targets;
        this.DecayCoefficient = decayCoefficent;
        this.TargetRadius = targetRadius;
        this.MaxAcceleration = maxAcceleration;
    }


    public SteeringOutput GetSteering(Vector3 characterPosition, float characterOrientation, Vector3 currentVelocity, float currentRotation, Vector3 targetPosition, float targetOrientation, Vector3 targetVelocity, float targetRotation)
    {
        SteeringOutput newSteering = new SteeringOutput();

        Vector3 separationDirection;

        float distanceToTarget;
        float separationStrength;

        separationStrength = 0;

        for (int i = 0; i < Targets.Count; ++i)
        {
            separationDirection = characterPosition - Targets[i].transform.position;
            distanceToTarget = separationDirection.magnitude;

            if(distanceToTarget < TargetRadius)
            {
                separationStrength = Mathf.Min(DecayCoefficient / (distanceToTarget * distanceToTarget), MaxAcceleration);

            }

            separationDirection.Normalize();
            newSteering.linearSpeed += separationStrength * separationDirection;


        }


        return newSteering;

    }

}
