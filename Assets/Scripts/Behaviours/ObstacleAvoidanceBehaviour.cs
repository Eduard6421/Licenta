using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidanceBehaviour : ISteerable
{

    private ISteerable SeekBehaviour;
    private float AvoidanceDistance;
    private float LookAhead;
    private SteeringOutput newSteering;

    public ObstacleAvoidanceBehaviour(float maxAcceleration, float avoidDistance, float lookAhead)
    {
        this.AvoidanceDistance = 2;
        this.LookAhead = 3;
        this.newSteering = new SteeringOutput();

        SeekBehaviour = new SeekBehaviour(maxAcceleration);
    }

    public SteeringOutput GetSteering(Vector3 characterPosition, float characterOrientation, Vector3 currentVelocity, float currentRotation, Vector3 targetPosition, float targetOrientation, Vector3 targetVelocity, float targetRotation)
    {
        newSteering.linearAcceleration = Vector3.zero;
        newSteering.angularAcceleration = 0;

        RaycastHit hit;
        Vector3 rayVector;
        Vector3 leftWhisker;
        Vector3 rightWhisker;

        rayVector = currentVelocity.normalized * LookAhead;
        leftWhisker = Quaternion.AngleAxis(20,Vector3.up) * currentVelocity.normalized * (LookAhead / 2);
        rightWhisker = Quaternion.AngleAxis(-20,Vector3.up)* currentVelocity.normalized * (LookAhead / 2);

        /*
        Debug.DrawRay(characterPosition, rayVector, Color.red);
        Debug.DrawRay(characterPosition, leftWhisker, Color.green);
        Debug.DrawRay(characterPosition, rightWhisker, Color.yellow);
        */
        if (Physics.Raycast(characterPosition, rayVector, out hit, LookAhead))
        {
            targetPosition = hit.point + hit.normal * AvoidanceDistance;
            newSteering = SeekBehaviour.GetSteering(characterPosition, characterOrientation, currentVelocity, currentRotation, targetPosition, targetOrientation, targetVelocity, targetRotation);
            //Debug.Log(newSteering.linearSpeed);
        }
        else if (Physics.Raycast(characterPosition, leftWhisker, out hit, LookAhead))
        {
            targetPosition = hit.point + hit.normal * AvoidanceDistance;
            newSteering = SeekBehaviour.GetSteering(characterPosition, characterOrientation, currentVelocity, currentRotation, targetPosition, targetOrientation, targetVelocity, targetRotation);
            //Debug.Log(newSteering.linearSpeed);
        }
        else if (Physics.Raycast(characterPosition, rightWhisker, out hit, LookAhead))
        {
            targetPosition = hit.point + hit.normal * AvoidanceDistance;
            newSteering = SeekBehaviour.GetSteering(characterPosition, characterOrientation, currentVelocity, currentRotation, targetPosition, targetOrientation, targetVelocity, targetRotation);
            //Debug.Log(newSteering.linearSpeed);
        }



        return newSteering;
    }



}
