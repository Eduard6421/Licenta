using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendedSteeringBehaviour : ISteerable
{
    private List<WeightedBehaviour> Behaviours;

    private float MaxAcceleration;
    private float MaxRotation;

    public BlendedSteeringBehaviour(float maxAcceleration, float maxRotation, List<WeightedBehaviour> behaviourList)
    {
        this.MaxAcceleration = maxAcceleration;
        this.MaxRotation = maxRotation;
        this.Behaviours = behaviourList;
    }


    public SteeringOutput GetSteering(Vector3 characterPosition, float characterOrientation, Vector3 currentVelocity, float currentRotation, Vector3 targetPosition, float targetOrientation, Vector3 targetVelocity, float targetRotation)
    {
        SteeringOutput newSteering = new SteeringOutput();
        SteeringOutput tempSteering = new SteeringOutput();

        for (int i = 0; i < Behaviours.Count; ++i)
        {
            tempSteering = Behaviours[i].Behaviour.GetSteering(characterPosition, characterOrientation, currentVelocity, currentRotation, targetPosition, targetOrientation, targetVelocity, targetRotation);
            newSteering.linearSpeed += tempSteering.linearSpeed * Behaviours[i].Weight;
            newSteering.angularSpeed = tempSteering.angularSpeed * Behaviours[i].Weight;
        }

        if(newSteering.linearSpeed.magnitude > MaxAcceleration)
        {
            newSteering.linearSpeed = newSteering.linearSpeed.normalized * MaxAcceleration;
        }

        if(Mathf.Abs(newSteering.angularSpeed) > MaxRotation)
        {
            newSteering.angularSpeed = (newSteering.angularSpeed / Mathf.Abs(newSteering.angularSpeed)) * MaxRotation;
        }

        return newSteering;

    }

}
