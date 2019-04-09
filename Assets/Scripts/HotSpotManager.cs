using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class HotSpotManager : MonoBehaviour
{
    public static HotSpotManager instance;
    private GoalManager GoalMaster;
    private bool HasOneHostspot;

    private Utilities.RandomizedResourceArray HotspotAvailability = new Utilities.RandomizedResourceArray();
    
    public static HotSpotManager GetInstance()
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
        }
        DontDestroyOnLoad(gameObject);
    }
    

    void Start()
    {
        GoalMaster = GoalManager.GetInstance();
    }


    public bool IsReady()
    {
        return GoalMaster != null;
    }


    public GameObject getRandomHotSpot()
    {
        string HotSpotId = HotspotAvailability.GetRandomResource();

        GameObject hotspot = GameObject.Find(HotSpotId);

        return hotspot;
    }


    public void AddHotSpot(string hotspotID, int numOfSpots)
    {
        HotspotAvailability.AddResource(hotspotID, numOfSpots);
        
        if (!HasOneHostspot)
        {
            HasOneHostspot = true;
            GoalMaster.SetSpawnPointsFlagOn();
            
        }

    }


}