using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneScript : MonoBehaviour
{

    public void GetBounds(out Vector3 leftBound , out Vector3 rightBound)
    {
        Renderer rend = GetComponent<Renderer>();

        Vector3 Center;
        Vector3 Extends;

        Center  = rend.bounds.center;
        Extends = rend.bounds.extents;

        leftBound = Center - Extends;
        rightBound = Center + Extends;
        
    }
}
