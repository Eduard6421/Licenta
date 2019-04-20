using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal
{
    public delegate void GoalHandler(GameObject target, Vector3 targetPosition, float interactionTime);

    public float InteractionTime { get; private set; }
    public GameObject InteractionObject { get; private set; }
    public Vector3 TargetPosition { get; private set; }
    public Queue<Vector3> TargetArray;
    public GoalHandler GoalFunction { get; private set; }

    public Goal(GoalHandler goalFunction, float interactionTime, Vector3 targetPosition, GameObject interactionObjct)
    {
        this.GoalFunction = goalFunction;
        this.InteractionTime = interactionTime;
        this.InteractionObject = interactionObjct;
        this.TargetPosition = targetPosition;
        this.TargetArray = new List<Vector3();
    }

    public bool UpdateTarget()
    {
        this.TargetPosition = NextPosition.Dequeue();
    }

}
