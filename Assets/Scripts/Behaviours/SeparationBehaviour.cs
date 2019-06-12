using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparationBehaviour : ISteerable
{

    private List<GameObject> Targets;

    private float MaxAcceleration;
    private float DecayCoefficient;
    private float TargetRadius;

    private SteeringOutput newSteering;

    public SeparationBehaviour(float maxAcceleration, float maxSpeed, float maxAngularAcceleration, float maxRotation, float slowRadius, float targetRadius, float timeToTarget, List<GameObject> targets, float decayCoefficent) 
    {
        this.Targets = targets;
        this.DecayCoefficient = decayCoefficent;
        this.TargetRadius = targetRadius;
        this.MaxAcceleration = maxAcceleration;
        this.newSteering = new SteeringOutput();
    }


    public SteeringOutput GetSteering(Vector3 characterPosition, float characterOrientation, Vector3 currentVelocity, float currentRotation, Vector3 targetPosition, float targetOrientation, Vector3 targetVelocity, float targetRotation)
    {
        Vector3 separationDirection;

        float distanceToTarget;
        float separationStrength;


        newSteering.linearAcceleration = Vector3.zero;
        newSteering.angularAcceleration = 0;
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
            newSteering.linearAcceleration += separationStrength * separationDirection;


        }


        return newSteering;

    }

}
