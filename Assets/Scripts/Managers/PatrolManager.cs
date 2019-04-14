using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolManager : MonoBehaviour
{

    public static PatrolManager instance;

    private Queue<List<GameObject>> PatrolRoutes;


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
        PatrolRoutes = new Queue<List<GameObject>>();
    }


    public void AddPatrolRoute(List<GameObject> PatrolRoute)
    {
        PatrolRoutes.Enqueue(PatrolRoute);
    }

    public List<GameObject> GetPatrolRoute()
    {
        return PatrolRoutes.Dequeue();
    }


}


