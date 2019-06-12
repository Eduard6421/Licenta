using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringOutput  {

    public Vector3 linearAcceleration { get; set; }
    public float angularAcceleration { get; set; }

    public SteeringOutput()
    {
        linearAcceleration = Vector3.zero;
        angularAcceleration = 0f;
    }

    public SteeringOutput(float angularAcceleration)
    {
        this.angularAcceleration = angularAcceleration;
    }

    public SteeringOutput(Vector3 linearAcceleration)
    {
        this.linearAcceleration = linearAcceleration;
    }

    public SteeringOutput(Vector3 linearAcceleration, float angularAcceleration)
    {
        this.linearAcceleration = linearAcceleration;
        this.angularAcceleration = angularAcceleration;
    }

}
