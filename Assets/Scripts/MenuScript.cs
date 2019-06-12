using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    private bool isEmergency;

    private bool done;
    private List<GameObject> emergencySpots;
    private GoalManager goalMaster;

    private GameObject statusText;

    private ReportScript reportScriptInstance;

    private float startTime;
    private float exitTime;

    private static MenuScript instance = null;


    public static MenuScript GetInstance()
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

        reportScriptInstance = ReportScript.GetInstance();
    }


    public void DeclareEmergency()
    {
        goalMaster.DeclareEmergencyState();
        reportScriptInstance.SimulationStartFlag();


        statusText = GameObject.Find("StatusText");
        TextMeshProUGUI textmest = statusText.GetComponent<TextMeshProUGUI>();
        textmest.SetText("Emergency");

        startTime = Time.time;
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
        
}
