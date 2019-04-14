using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendedSteeringBehaviour : ISteerable
{
    private List<WeightedBehaviour> Behaviours;
    private List<SteeringOutput> previousOutputs;

    private Vector3 CurrentRVOSpeed;
    private float RVOWeight;
    private float MaxSpeed = 5;
    private float MaxRotation;


    public BlendedSteeringBehaviour(float maxSpeed, float maxRotation, List<WeightedBehaviour> behaviourList)
    {
        this.MaxSpeed = maxSpeed;
        this.MaxRotation = maxRotation;
        this.Behaviours = behaviourList;
        this.RVOWeight = 0;
    }

    private void UpdateRVOWeight(Vector3 RVOVelocity)
    {
        if ((CurrentRVOSpeed - RVOVelocity).magnitude > 3f)
        {
            RVOWeight = 0.8f;
        }
        else
        {
            RVOWeight = 0.2f;
        }

    }



    public void setRVOVelocity(Vector3 newRVOVelocity)
    {
        UpdateRVOWeight(newRVOVelocity);
        this.CurrentRVOSpeed = newRVOVelocity;
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

        if(newSteering.linearSpeed.magnitude > MaxSpeed)
        {
            newSteering.linearSpeed = newSteering.linearSpeed.normalized * MaxSpeed;
        }

        if(Mathf.Abs(newSteering.angularSpeed) > MaxRotation)
        {
            newSteering.angularSpeed = (newSteering.angularSpeed / Mathf.Abs(newSteering.angularSpeed)) * MaxRotation;
        }

        newSteering.linearSpeed = (1 - RVOWeight) * newSteering.linearSpeed + CurrentRVOSpeed;

        return newSteering;
    }

}
