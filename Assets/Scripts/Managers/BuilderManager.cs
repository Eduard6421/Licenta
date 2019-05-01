using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderManager : MonoBehaviour
{

    private static BuilderManager instance = null;
    private static CivilianBuilder civilianBuilder = null;

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
        civilianBuilder = new CivilianBuilder();

    }

    public  CivilianBuilder GetCivilianBuilder()
    {
        return civilianBuilder;
    }


    public static BuilderManager GetInstance()
    {
        return instance;
    }

}
