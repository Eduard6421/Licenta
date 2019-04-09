using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotSpot : MonoBehaviour
{
    [SerializeField]
    [Range(0,4)]
    private int MaxQueue = 1;

    void Start()
    {
        StartCoroutine(HotSpotSleep());
    }

    IEnumerator HotSpotSleep()
    {
        yield return new WaitUntil(() => HotSpotManager.GetInstance().IsReady() == true);
        HotSpotManager.GetInstance().AddHotSpot(this.name, MaxQueue);

    }


}
