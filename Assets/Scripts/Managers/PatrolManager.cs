using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolManager : MonoBehaviour
{

    public static PatrolManager instance;

    private Queue<List<Vector3>> PatrolRoutes;

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
        PatrolRoutes = new Queue<List<Vector3>>();
    }

    public static PatrolManager GetInstance()
    {
        return instance;
    }

    public void AddPatrolRoute(List<Vector3> PatrolRoute)
    {
        PatrolRoutes.Enqueue(PatrolRoute);
    }

    public List<Vector3> GetPatrolRoute()
    {
        return PatrolRoutes.Dequeue();
    }


}


