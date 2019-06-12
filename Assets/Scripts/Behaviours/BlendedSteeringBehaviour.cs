using System.Collections.Generic;
using UnityEngine;

public class BlendedSteeringBehaviour : ISteerable
{
    private List<WeightedBehaviour> Behaviours;
    private List<SteeringOutput> previousOutputs;

    private Vector3 CurrentRVOSpeed;
    private float RVOWeight;
    private float DifferenceRVOWeight;
    private float MaxSpeed = 5;
    private float MaxRotation;
    private SteeringOutput newSteering;
    private SteeringOutput tempSteering;

    public BlendedSteeringBehaviour(float maxSpeed, float maxRotation, List<WeightedBehaviour> behaviourList)
    {
        this.MaxSpeed = maxSpeed;
        this.MaxRotation = maxRotation;
        this.Behaviours = behaviourList;
        this.DifferenceRVOWeight = 0;
        this.newSteering = new SteeringOutput();
        this.tempSteering = new SteeringOutput();
    }

    private void UpdateRVOWeight(Vector3 RVOVelocity)
    {
        if ((CurrentRVOSpeed - RVOVelocity).magnitude > 3f)
        {
            RVOWeight = 0.6f;
        }
        else
        {
            RVOWeight = 0.4f;
        }

    }


    public void setRVOVelocity(Vector3 newRVOVelocity)
    {
        UpdateRVOWeight(newRVOVelocity);
        this.CurrentRVOSpeed = newRVOVelocity;
    }

    public void NewWeightUpdate(Vector3 velocity)
    {
        if ((velocity - CurrentRVOSpeed).magnitude > 2)
        {
            RVOWeight = 0.8f;
        }
        else
            RVOWeight = 0.2f;
    }



    public SteeringOutput GetSteering(Vector3 characterPosition, float characterOrientation, Vector3 currentVelocity, float currentRotation, Vector3 targetPosition, float targetOrientation, Vector3 targetVelocity, float targetRotation)
    {


        newSteering.linearAcceleration = Vector3.zero;
        newSteering.angularAcceleration = 0;

        tempSteering.linearAcceleration = Vector3.zero;
        tempSteering.angularAcceleration = 0;



        for (int i = 0; i < Behaviours.Count; ++i)
        {
            tempSteering = Behaviours[i].Behaviour.GetSteering(characterPosition, characterOrientation, currentVelocity, currentRotation, targetPosition, targetOrientation, targetVelocity, targetRotation);

            newSteering.linearAcceleration += tempSteering.linearAcceleration * Behaviours[i].Weight;
            newSteering.angularAcceleration += tempSteering.angularAcceleration * Behaviours[i].Weight;
        }

        if(newSteering.linearAcceleration.magnitude > MaxSpeed)
        {
            newSteering.linearAcceleration = newSteering.linearAcceleration.normalized * MaxSpeed;
        }

        if(Mathf.Abs(newSteering.angularAcceleration) > MaxRotation)
        {
            newSteering.angularAcceleration = (newSteering.angularAcceleration / Mathf.Abs(newSteering.angularAcceleration)) * MaxRotation;
        }

        NewWeightUpdate(newSteering.linearAcceleration);

        newSteering.linearAcceleration = (1 - RVOWeight) * newSteering.linearAcceleration + CurrentRVOSpeed* RVOWeight;


        return newSteering;
    }

}
