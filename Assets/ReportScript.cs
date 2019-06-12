using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportScript : MonoBehaviour
{

    private int numAgents;
    private bool emergencyBegan;
    private bool ended = false;
    private float startTime;

    private static ReportScript instance = null;


    public static ReportScript GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }


    public void SetAgentsNumber(int agentNum)
    {
        this.numAgents = agentNum;
    }

    public void SimulationStartFlag()
    {
        emergencyBegan = true;
        startTime = Time.time;
        ended = true;
    }

    public void ReportDeletedAgent()
    {
        this.numAgents--;

        if (numAgents == 0)
        {
            Debug.Log(Time.time - startTime);
            Debug.Log("Simulation has ended");
        }
    }
}
