using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void GoalHandler();

public class Goal
{

    public float GoalActionTime { get; private set; }
    public List<GameObject> InteractionObjects { get; private set; }
    public List<Vector3> TargetPositions { get; private set; }
    public GoalHandler GoalFunction { get; private set; }

    private bool RightFlow;
    private int CurrentTargetIndex;

    public Goal(GoalHandler goalFunction, float goalActionTime, List<Vector3> targetPosition, List<GameObject> interactionObject)
    {
        this.GoalFunction = goalFunction;
        this.GoalActionTime = goalActionTime;
        this.InteractionObjects = interactionObject;
        this.TargetPositions = targetPosition;
        this.CurrentTargetIndex = 0;
        this.RightFlow = true;
    }


    public GameObject GetCurrentTargetObject()
    {
        return InteractionObjects[CurrentTargetIndex];
    }

    public Vector3 GetCurrentTargetPosition()
    {
        return TargetPositions[CurrentTargetIndex];
    }


    public bool UpdateSequentialTarget()
    {
        if(this.TargetPositions.Count > 0)
        {
            return false;
        }

        if (this.CurrentTargetIndex < this.TargetPositions.Count - 1)
        {
            CurrentTargetIndex++;
            return true;
        }


        return false;
    }
    public void UpdatePatrolTarget()
    {
        if(this.CurrentTargetIndex == this.TargetPositions.Count - 1)
        {
            RightFlow = false;
        }
        if(this.CurrentTargetIndex == 0)
        {
            RightFlow = true;
        }

        if(RightFlow)
        {
            this.CurrentTargetIndex++;
        }
        else
        {
            this.CurrentTargetIndex--;
        }
 
    }


}
