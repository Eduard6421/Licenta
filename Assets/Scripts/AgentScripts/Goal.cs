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


    public Goal()
    {
        this.GoalFunction = null;
        this.GoalActionTime = 0;
        this.InteractionObjects = null;
        this.TargetPositions = null;
        this.CurrentTargetIndex = 0;
        this.RightFlow = true;
    }

    public Goal(float goalActionTime, List<Vector3> targetPositions, List<GameObject> interactionObject)
    {
        this.GoalFunction = null;
        this.GoalActionTime = goalActionTime;
        this.InteractionObjects = interactionObject;
        this.TargetPositions = targetPositions;
        this.CurrentTargetIndex = 0;
        this.RightFlow = true;
    }


    public Goal(GoalHandler goalFunction, float goalActionTime, List<Vector3> targetPositions, List<GameObject> interactionObject)
    {
        this.GoalFunction = goalFunction;
        this.GoalActionTime = goalActionTime;
        this.InteractionObjects = interactionObject;
        this.TargetPositions = targetPositions;
        this.CurrentTargetIndex = 0;
        this.RightFlow = true;
    }


    public Goal(Goal otherGoal, GoalHandler goalFunction)
    {
        this.GoalFunction = goalFunction;
        this.GoalActionTime = otherGoal.GoalActionTime;
        this.InteractionObjects = otherGoal.InteractionObjects;
        this.TargetPositions = otherGoal.TargetPositions;
        this.CurrentTargetIndex = otherGoal.CurrentTargetIndex;
        this.RightFlow = otherGoal.RightFlow;
    }


    public GameObject GetCurrentTargetObject()
    {
        if (InteractionObjects.Count == 0)
            return null;

        return InteractionObjects[CurrentTargetIndex];
    }

    public Vector3 GetCurrentTargetPosition()
    {
        return TargetPositions[CurrentTargetIndex];
    }

    public bool IsEnded()
    {
        return this.CurrentTargetIndex == this.TargetPositions.Count;
    }

    public List<Vector3> GetCornerArray()
    {
        return this.TargetPositions;
    }


    public bool UpdateSequentialTargetObject()
    {
        if(TargetPositions.Count == 1)
        {
            return true;
        }

        CurrentTargetIndex++;

        if (this.CurrentTargetIndex >= this.TargetPositions.Count - 1)
        {
            return true;
        }

        return false;
    }


    public bool UpdateSequentialTarget()
    {

        CurrentTargetIndex++;

        if (this.CurrentTargetIndex == this.TargetPositions.Count - 1)
        {
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
