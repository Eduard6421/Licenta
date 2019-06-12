using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyManager : MonoBehaviour
{

    [SerializeField]
    private bool isEmergency;

    private bool done;
    private List<GameObject> emergencySpots;
    private GoalManager goalMaster;

    private static EmergencyManager instance = null;


    public static EmergencyManager GetInstance()
    {
        return instance;
    }


    private void Awake()
    {

        done = false;
        goalMaster = (GoalManager)GameObject.FindObjectOfType<GoalManager>();

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

    }




    private void Update()
    {

        if(isEmergency)
        {

            if(!done)
            {
                done = true;
                goalMaster.DeclareEmergencyState();
            }
        }

    }



}
