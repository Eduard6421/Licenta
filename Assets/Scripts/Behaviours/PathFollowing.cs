using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowing : ISteerable
{
    private int CurrentTarget;
    private float TargetRadius;
    private SteeringOutput newSteering;

    private List<Vector3> Path;
    private ISteerable SeekBehaviour;
    private ISteerable LookAndRunBehaviour;
    private ISteerable FaceBehaviour;


    public PathFollowing(float maxAcceleration, float maxSpeed, float maxAngularAcceleration, float maxRotation, float slowRadius, float targetRadius, float timeToTarget, List<Vector3> path)
    {
        this.TargetRadius = targetRadius;
        this.Path = path;
        this.CurrentTarget = 0;
        this.newSteering = new SteeringOutput();

        LookAndRunBehaviour = new LookAndRunBehaviour(maxAcceleration, maxSpeed, maxAngularAcceleration, maxRotation, slowRadius, targetRadius, timeToTarget);
        SeekBehaviour = new SeekBehaviour(maxAcceleration);
        FaceBehaviour = new FaceBehaviour(maxAcceleration, maxSpeed, maxAngularAcceleration, maxRotation, slowRadius, targetRadius, timeToTarget);

    }


    public SteeringOutput GetSteering(Vector3 characterPosition, float characterOrientation, Vector3 currentVelocity, float currentRotation, Vector3 targetPosition, float targetOrientation, Vector3 targetVelocity, float targetRotation)
    {
        newSteering.linearAcceleration = Vector3.zero;
        newSteering.angularAcceleration = 0;
        

        if (Path.Count > 0)
        {
            Vector3 distanceVector = Path[CurrentTarget] - characterPosition;


            if (distanceVector.magnitude < TargetRadius && CurrentTarget < Path.Count - 1)
            {
                ++CurrentTarget;
            }

            if (CurrentTarget < Path.Count - 1)
            {
                newSteering = SeekBehaviour.GetSteering(characterPosition, characterOrientation, currentVelocity, currentRotation, Path[CurrentTarget], targetOrientation, targetVelocity, targetRotation);
                newSteering.angularAcceleration = FaceBehaviour.GetSteering(characterPosition, characterOrientation, currentVelocity, currentRotation, Path[CurrentTarget], targetOrientation, targetVelocity, targetRotation).angularAcceleration;
            }
            else
            {
                newSteering = LookAndRunBehaviour.GetSteering(characterPosition, characterOrientation, currentVelocity, currentRotation, Path[CurrentTarget], targetOrientation, targetVelocity, targetRotation);
            }
        }


        // Debug.Log(newSteering.angularAcceleration);
        return newSteering;


    }




}
