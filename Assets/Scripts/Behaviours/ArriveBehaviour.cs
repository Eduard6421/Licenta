using UnityEngine;

public class ArriveBehaviour : CharacterBehaviour
{

    public ArriveBehaviour(float maxAcceleration, float maxSpeed,float maxAngularAcceleration, float maxRotation,float slowRadius, float targetRadius, float timeToTarget) : base(maxAcceleration, maxSpeed, maxAngularAcceleration, maxRotation,  slowRadius, targetRadius, timeToTarget)
    {

    }

    public override SteeringOutput GetSteering(Vector3 characterPosition, float characterOrientation, Vector3 currentVelocity, float currentRotation, Vector3 targetPosition, float targetOrientation, Vector3 targetVelocity, float targetRotation)
    {
        SteeringOutput newSteering = new SteeringOutput();

        Vector3 DesiredDirection;
        Vector3 DesiredVelocity;

        float distanceToTarget;
        float targetSpeed;

        DesiredDirection = targetPosition - characterPosition;
        distanceToTarget = DesiredDirection.magnitude;

        if (distanceToTarget < TargetRadius)
        {
            return newSteering;
        }

        if (distanceToTarget > SlowRadius)
        {
            targetSpeed = MaxSpeed;
        }
        else
        {
            targetSpeed = MaxSpeed * distanceToTarget / SlowRadius;
        }

        DesiredVelocity = DesiredDirection;
        DesiredVelocity.Normalize();
        DesiredVelocity = DesiredVelocity * targetSpeed;

        newSteering.linearSpeed = DesiredVelocity - currentVelocity;
        newSteering.linearSpeed /= TimeToTarget;

        if(newSteering.linearSpeed.magnitude > MaxAcceleration)
        {
            newSteering.linearSpeed.Normalize();
            newSteering.linearSpeed *= MaxAcceleration;
        }

        newSteering.linearSpeed = newSteering.linearSpeed * targetSpeed;

        
         return newSteering;
        
    }

}
