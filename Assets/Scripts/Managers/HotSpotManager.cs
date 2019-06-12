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
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    

    void Start()
    {
        GoalMaster = GoalManager.GetInstance();

        HotSpot hotspotComponent;

        List<GameObject> HotSpots = GameObject.FindGameObjectsWithTag("HotSpot").ToList<GameObject>();
        for(int i = 0; i < HotSpots.Count; ++i)
        {
            hotspotComponent = HotSpots[i].GetComponent<HotSpot>();
            AddHotSpot(HotSpots[i].name, hotspotComponent.GetMaxQueue());
        }

    }


    public GameObject getRandomHotSpot()
    {
        string HotSpotId = HotspotAvailability.GetRandomResource();

        GameObject hotspot = GameObject.Find(HotSpotId);

        return hotspot;
    }

    public GameObject getRandomHotSpot(int numOfSpots)
    {
        string HotSpotId = HotspotAvailability.GetRandomResource(numOfSpots);

        GameObject hotspot = GameObject.Find(HotSpotId);

        return hotspot;
    }

    public void UpdateHotSpot(string hotspotID, int numOfSpots)
    {
        HotspotAvailability.UpdateResource(hotspotID, numOfSpots);
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