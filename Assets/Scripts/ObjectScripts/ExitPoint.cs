using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPoint : MonoBehaviour
{
    [SerializeField]
    private GameObject parentExitPoint;

    public GameObject GetParent()
    {
        return parentExitPoint;
    }
}
