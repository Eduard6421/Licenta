using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBehaviour : ISteerable
{

    protected float MaxAcceleration { get; set; }
    protected float MaxSpeed { get; set; }
    protected float MaxAngularAcceleration { get; set; }
    protected float MaxRotation { get; set; }
    protected float SlowRadius { get; set; }
    protected float TargetRadius { get; set; }
    protected float TimeToTarget { get; set; }


    protected CharacterBehaviour(float maxAcceleration)
    {
        this.MaxAcceleration = maxAcceleration;
    }

    protected CharacterBehaviour(float maxAcceleration, float maxSpeed, float maxAngularAcceleration, float maxRotation, float slowRadius, float targetRadius, float timeToTarget)
    {
        this.MaxAcceleration = maxAcceleration;
        this.MaxSpeed = maxSpeed;
        this.MaxAngularAcceleration = maxAngularAcceleration;
        this.MaxRotation = maxRotation;
        this.SlowRadius = slowRadius;
        this.TargetRadius = targetRadius;
        this.TimeToTarget = timeToTarget;

    }

    public abstract SteeringOutput GetSteering(Vector3 characterPosition, float characterOrientation, Vector3 currentVelocity, float currentRotation, Vector3 targetPosition, float targetOrientation, Vector3 targetVelocity, float targetRotation);
}
