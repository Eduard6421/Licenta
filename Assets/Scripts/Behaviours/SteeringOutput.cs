using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringOutput  {

    public Vector3 linearSpeed { get; set; }
    public float angularSpeed { get; set; }

    public SteeringOutput()
    {
        linearSpeed = Vector3.zero;
        angularSpeed = 0f;
    }

    public SteeringOutput(float angularSpeed)
    {
        this.angularSpeed = angularSpeed;
    }

    public SteeringOutput(Vector3 linearSpeed)
    {
        this.linearSpeed = linearSpeed;
    }

    public SteeringOutput(Vector3 linearSpeed, float angularSpeed)
    {
        this.linearSpeed = linearSpeed;
        this.angularSpeed = angularSpeed;
    }

}
