using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedBehaviour
{
    public ISteerable Behaviour { get; set; }
    public float Weight { get; set; }

    public WeightedBehaviour(ISteerable behaviour, float weight)
    {
        this.Behaviour = behaviour;
        this.Weight = weight;
    }


}