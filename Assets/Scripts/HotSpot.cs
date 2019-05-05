using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotSpot : MonoBehaviour
{
    [SerializeField]
    [Range(0, 7)]
    private int MaxQueue;

    public int GetMaxQueue()
    {
        return MaxQueue;
    }

}
