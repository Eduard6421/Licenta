using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject AgentPrefab;


    private GameObject NewAgent;
    private List<GameObject> SpawnPoints;
    private List<GameObject> Agents;
    private SpawnArea SpawnScript;

    private GoalManager BehaviourMaster;

    private int AgentCount = 1;

    private Vector3 RandomSpawnLocation;

    public static EntitySpawner instance;
    
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
        float newX;
        float newZ;
        float newMaxZ;
        
       AgentPrefab = Resources.Load("RobotPrefab", typeof(GameObject)) as GameObject;

        BehaviourMaster = GameObject.Find("BehaviourMaster").GetComponent<GoalManager>();

        Agents = new List<GameObject>();

        SpawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("Spawn"));

        for (int i = 0; i < SpawnPoints.Count; ++i)
        {
            SpawnScript = SpawnPoints[i].GetComponent<SpawnArea>();
            
            for (int j = 0; j < SpawnScript.spawnDensity; ++j)
            {
                RandomSpawnLocation = new Vector3(SpawnPoints[i].transform.position.x, SpawnPoints[i].transform.position.y, SpawnPoints[i].transform.position.z);
                //Debug.Log(SpawnPoints[i].transform.position.x + " " + SpawnPoints[i].transform.position.z);

                newX = Random.Range(-Mathf.Floor(SpawnScript.spawnRange), Mathf.Floor(SpawnScript.spawnRange));
                newMaxZ = Mathf.Sqrt(Mathf.Pow(SpawnScript.spawnRange,2) - Mathf.Pow(newX,2)    );
                newZ = Random.Range(-newMaxZ, newMaxZ);

                RandomSpawnLocation += new Vector3(newX, 0, newZ);
                NewAgent = (GameObject)Instantiate(AgentPrefab, RandomSpawnLocation, transform.rotation);

                
                NewAgent.name = "Agent" + AgentCount;
                NewAgent.tag = "Agent";
                AgentCount++;
            }

        }

    }
}
