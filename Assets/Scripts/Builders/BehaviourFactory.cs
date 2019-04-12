using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BehaviourFactory
{

    public static ISteerable GetCharacterBehaviour(Utilities.Behaviour selectedBehaviour, float maxSpeed, float maxRotation, List<Vector3> CharacterPath, List<GameObject> Entities)
    {

        ISteerable steeringType;

        float MaxAcceleration;
        float MaxSpeed;
        float MaxAngularAcceleration;
        float MaxRotation;
        float SlowRadius;
        float TargetRadius;
        float TimeToTarget;
        float MaxPrediction;
        float WanderOffset;
        float WanderRadius;
        float WanderRate;
        float WanderOrientation;
        float DecayCoefficent;
        float LookAhead;
        float AvoidanceDistance;


        switch (selectedBehaviour)
        {
            case Utilities.Behaviour.Align:
                MaxAcceleration = 0.05f;
                MaxSpeed = 8f;
                MaxAngularAcceleration = 0.1f;
                MaxRotation = 1.5f;
                SlowRadius = 10f;
                TargetRadius = 2f;
                TimeToTarget = 0.5f;
                MaxPrediction = 2f;

                steeringType = new AlignBehaviour(MaxAcceleration, MaxSpeed, MaxAngularAcceleration, MaxRotation, SlowRadius, TargetRadius, TimeToTarget);
                break;

            case Utilities.Behaviour.AlignOpposite:

                MaxAcceleration = 0.05f;
                MaxSpeed = 8f;
                MaxAngularAcceleration = 0.1f;
                MaxRotation = 1.5f;
                SlowRadius = 10f;
                TargetRadius = 2f;
                TimeToTarget = 0.5f;
                MaxPrediction = 2f;


                steeringType = new AlignOppositeBehaviour(MaxAcceleration, MaxSpeed, MaxAngularAcceleration, MaxRotation, SlowRadius, TargetRadius, TimeToTarget);
                break;


            case Utilities.Behaviour.Arrive:

                MaxAcceleration = 0.05f;
                MaxSpeed = 8f;
                MaxAngularAcceleration = 0.1f;
                MaxRotation = 1.5f;
                SlowRadius = 15f;
                TargetRadius = 1f;
                TimeToTarget = 0.5f;
                MaxPrediction = 2f;

                steeringType = new ArriveBehaviour(MaxAcceleration, MaxSpeed, MaxAngularAcceleration, MaxRotation, SlowRadius, TargetRadius, TimeToTarget);
                break;


            case Utilities.Behaviour.CollisionAvoidance:

                MaxAcceleration = 0.05f;
                MaxSpeed = 8f;
                MaxAngularAcceleration = 0.1f;
                MaxRotation = 1.5f;
                SlowRadius = 15f;
                TargetRadius = 4f;
                TimeToTarget = 1f;
                MaxPrediction = 2f;

                steeringType = new CollisionAvoidanceBehaviour(MaxAcceleration, MaxSpeed, MaxAngularAcceleration, MaxRotation, SlowRadius, TargetRadius, TimeToTarget, Entities);
                break;


            case Utilities.Behaviour.Evade:

                MaxAcceleration = 0.05f;
                MaxSpeed = 8f;
                MaxAngularAcceleration = 0.1f;
                MaxRotation = 1.5f;
                SlowRadius = 15f;
                TargetRadius = 1f;
                TimeToTarget = 0.5f;
                MaxPrediction = 1f;

                steeringType = new EvadeBehaviour(MaxAcceleration, MaxSpeed, MaxAngularAcceleration, MaxRotation, SlowRadius, TargetRadius, TimeToTarget, MaxPrediction);
                break;

            case Utilities.Behaviour.Face:
                MaxAcceleration = 0.05f;
                MaxSpeed = 8f;
                MaxAngularAcceleration = 0.1f;
                MaxRotation = 1.5f;
                SlowRadius = 10f;
                TargetRadius = 2f;
                TimeToTarget = 0.5f;
                MaxPrediction = 2f;

                steeringType = new FaceBehaviour(MaxAcceleration, MaxSpeed, MaxAngularAcceleration, MaxRotation, SlowRadius, TargetRadius, TimeToTarget);
                break;


            case Utilities.Behaviour.Leave:

                MaxAcceleration = 0.05f;
                MaxSpeed = 8f;
                MaxAngularAcceleration = 0.1f;
                MaxRotation = 1.5f;
                SlowRadius = 15f;
                TargetRadius = 1f;
                TimeToTarget = 0.5f;
                MaxPrediction = 2f;

                steeringType = new LeaveBehaviour(MaxAcceleration, MaxSpeed, MaxAngularAcceleration, MaxRotation, SlowRadius, TargetRadius, TimeToTarget);
                break;

            case Utilities.Behaviour.LookAndRun:

                MaxAcceleration = 0.05f;
                MaxSpeed = 8f;
                MaxAngularAcceleration = 0.025f;
                MaxRotation = 1.5f;
                SlowRadius = 3f;
                TargetRadius = 1f;
                TimeToTarget = 0.5f;
                MaxPrediction = 2f;

                steeringType = new LookAndRunBehaviour(MaxAcceleration, MaxSpeed, MaxAngularAcceleration, MaxRotation, SlowRadius, TargetRadius, TimeToTarget);
                break;

            case Utilities.Behaviour.ObstacleAvoidance:

                MaxAcceleration = 4f;
                MaxSpeed = 8f;
                MaxAngularAcceleration = 0.025f;
                MaxRotation = 1.5f;
                SlowRadius = 3f;
                TargetRadius = 1f;
                TimeToTarget = 0.5f;
                MaxPrediction = 2f;
                AvoidanceDistance = 1f;
                LookAhead = 2f;

                steeringType = new ObstacleAvoidanceBehaviour(MaxAcceleration, AvoidanceDistance, LookAhead);
                break;



            case Utilities.Behaviour.PathFollowing:


                MaxAcceleration = 0.01f;
                MaxSpeed = 3f;
                MaxAngularAcceleration = 0.025f;
                MaxRotation = 1.5f;
                SlowRadius = 15f;
                TargetRadius = 1f;
                TimeToTarget = 0.5f;
                MaxPrediction = 2f;

                steeringType = new PathFollowing(MaxAcceleration, MaxSpeed, MaxAngularAcceleration, MaxRotation, SlowRadius, TargetRadius, TimeToTarget, CharacterPath);
                break;


            case Utilities.Behaviour.Separation:

                MaxAcceleration = 0.05f;
                MaxSpeed = 8f;
                MaxAngularAcceleration = 0.025f;
                MaxRotation = 1.5f;
                SlowRadius = 3f;
                TargetRadius = 1f;
                TimeToTarget = 0.5f;
                MaxPrediction = 2f;
                DecayCoefficent = 1f;
                steeringType = new SeparationBehaviour(MaxAcceleration, MaxSpeed, MaxAngularAcceleration, MaxRotation, SlowRadius, TargetRadius, TimeToTarget, Entities, DecayCoefficent);
                break;

            case Utilities.Behaviour.Pursue:

                MaxAcceleration = 0.05f;
                MaxSpeed = 8f;
                MaxAngularAcceleration = 0.1f;
                MaxRotation = 1.5f;
                SlowRadius = 15f;
                TargetRadius = 1f;
                TimeToTarget = 0.5f;
                MaxPrediction = 1f;


                steeringType = new PursueBehaviour(MaxAcceleration, MaxSpeed, MaxAngularAcceleration, MaxRotation, SlowRadius, TargetRadius, TimeToTarget, MaxPrediction);
                break;

            case Utilities.Behaviour.VelocityMatch:

                MaxAcceleration = 0.05f;
                MaxSpeed = 8f;
                MaxAngularAcceleration = 0.1f;
                MaxRotation = 1.5f;
                SlowRadius = 15f;
                TargetRadius = 1f;
                TimeToTarget = 0.5f;
                MaxPrediction = 2f;


                steeringType = new VelocityMatchBehaviour(MaxAcceleration, MaxSpeed, MaxAngularAcceleration, MaxRotation, SlowRadius, TargetRadius, TimeToTarget);
                break;



            case Utilities.Behaviour.Wander:

                MaxAcceleration = 0.03f;
                MaxSpeed = 1.5f;
                MaxAngularAcceleration = 0.025f;
                MaxRotation = 1.5f;
                SlowRadius = 15f;
                TargetRadius = 1f;
                TimeToTarget = 0.5f;
                MaxPrediction = 2f;
                WanderOffset = 1f;
                WanderRadius = 1f;
                WanderRate = 1000f;
                WanderOrientation = 0;

                steeringType = new WanderBehaviour(MaxAcceleration, MaxSpeed, MaxAngularAcceleration, MaxRotation, SlowRadius, TargetRadius, TimeToTarget, WanderOffset, WanderRadius, WanderRate, WanderOrientation);
                break;


            default:

                Debug.LogError("The given behaviour is unknown");

                MaxAcceleration = 0.05f;
                MaxSpeed = 8f;
                MaxAngularAcceleration = 0.1f;
                MaxRotation = 1.5f;
                SlowRadius = 15f;
                TargetRadius = 1f;
                TimeToTarget = 0.5f;
                MaxPrediction = 1f;
                WanderOffset = 1f;
                WanderRadius = 4f;
                WanderRate = 1f;
                WanderOrientation = 1;

                steeringType = new ArriveBehaviour(MaxAcceleration, MaxSpeed, MaxAngularAcceleration, MaxRotation, SlowRadius, TargetRadius, TimeToTarget);
                break;

        }


        return steeringType;

    }


}
