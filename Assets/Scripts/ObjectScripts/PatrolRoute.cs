using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolRoute : MonoBehaviour
{
    private List<Vector3> checkpointPositions;
    private PatrolManager manager;

    void Awake()
    {
        checkpointPositions = new List<Vector3>();

        for(int i = 0; i < this.transform.childCount; ++i)
        {
            checkpointPositions.Add(this.transform.GetChild(i).position);
        }

        manager = PatrolManager.GetInstance();
    }

    private void Start()
    {
        manager.AddPatrolRoute(checkpointPositions);       
    }


}