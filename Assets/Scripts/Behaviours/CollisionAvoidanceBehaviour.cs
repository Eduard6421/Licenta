using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidanceBehaviour : ISteerable
{
    private List<GameObject> Entities;


    private ISteerable EvadeBehaviour;
    private float MaxAcceleration;
    private float TargetRadius;
    private SteeringOutput newSteering;

    public CollisionAvoidanceBehaviour(float maxAcceleration, float maxSpeed, float maxAngularAcceleration, float maxRotation, float slowRadius, float targetRadius, float timeToTarget, List<GameObject> targets)
    {
        float maxPrediction = 2f;

        this.Entities = targets;
        this.TargetRadius = 1.2f;
        this.MaxAcceleration = 2.5f;
        this.newSteering = new SteeringOutput();

        EvadeBehaviour = new EvadeBehaviour(.5f, maxSpeed, maxAngularAcceleration, maxRotation, slowRadius, targetRadius, timeToTarget, maxPrediction);

    }


    public SteeringOutput GetSteering(Vector3 characterPosition, float characterOrientation, Vector3 currentVelocity, float currentRotation, Vector3 targetPosition, float targetOrientation, Vector3 targetVelocity, float targetRotation)
    {
        //target rotation is not needed

        newSteering.linearAcceleration = Vector3.zero;
        newSteering.angularAcceleration = 0;

        GameObject firstTarget = null;



        Vector3 firstTargetVelocity;
        Vector3 firstRelativePosition;
        Vector3 firstRelativeVelocity;
        Vector3 relativePosition;
        Vector3 relativeVelocity;

        float firstMinSeparation;
        float firstDistance;
        float currentDistance;
        float shortestTime;
        float minSeparation;
        float relativeSpeed;
        float timeToCollision;

        firstMinSeparation = 0;
        firstDistance = 0;

        firstRelativePosition = Vector3.zero;
        firstRelativeVelocity = Vector3.zero;
        firstTargetVelocity = Vector3.zero;

        shortestTime = Mathf.Infinity;



        for (int i = 0; i < Entities.Count; ++i)
        {
            relativePosition = Entities[i].transform.position - characterPosition;
            targetVelocity = Entities[i].GetComponent<KinematicEntity>().GetVelocity();

            relativeVelocity = targetVelocity - currentVelocity;
            relativeSpeed = relativeVelocity.magnitude;


            timeToCollision = (-1) * Vector3.Dot(relativePosition, relativeVelocity) / (relativeSpeed * relativeSpeed);

            currentDistance = relativePosition.magnitude;

            minSeparation = currentDistance - relativeSpeed * timeToCollision;

            if (minSeparation > 2 * TargetRadius)
                continue;


            if (timeToCollision > 0 && timeToCollision < shortestTime)
            {
                shortestTime = timeToCollision;
                firstTarget = Entities[i];
                firstMinSeparation = minSeparation;
                firstDistance = currentDistance;
                firstRelativePosition = relativePosition;
                firstRelativeVelocity = relativeVelocity;
                firstTargetVelocity = targetVelocity;
            }
        }

        if (firstTarget == null)
        {
            return newSteering;
        }


        if(Vector3.Dot(currentVelocity.normalized,firstTargetVelocity.normalized) < -0.8f)
        {
            newSteering = EvadeBehaviour.GetSteering(characterPosition, characterOrientation, currentVelocity, currentRotation, firstTarget.transform.position, targetOrientation, firstTargetVelocity, targetRotation);
        }
        else
        {
            if (firstMinSeparation <= 0 || firstDistance < 2 * TargetRadius)
            {
                relativePosition = characterPosition - firstTarget.transform.position;
            }
            else
            {
                relativePosition = firstRelativePosition + firstRelativeVelocity * shortestTime;

            }

            relativePosition.Normalize();

            newSteering.linearAcceleration = relativePosition * MaxAcceleration;

        }

        return newSteering;

    }


}
