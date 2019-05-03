using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroupManager : MonoBehaviour
{
    private static GroupManager instance;

    private int CurrentGroup;

    [SerializeField]
    private int MaxGroupNumber;
    private int GroupNumber;

    [SerializeField]
    private int MaxGroupMembers;
    private List<int> MembersToCreate;
    private SortedList<int,List<GameObject>> AgentGroups;

    public static GroupManager GetInstance()
    {
        return instance;    
    }


    public int GetNextGroup(GameObject agent)
    {
        int groupIndex;

        if (this.CurrentGroup == MembersToCreate.Count - 1)
        {
            BehaviourProbabilities.StopGroupDistributon();
        }

        groupIndex = MembersToCreate[this.CurrentGroup];
        AgentGroups[this.CurrentGroup].Add(agent);

        this.CurrentGroup++;

        return groupIndex;

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

        AgentGroups = new SortedList<int, List<GameObject>>();
    }


    void CreateGroupDistribution() {

        CurrentGroup = 0;
        GroupNumber = Random.Range(0, MaxGroupNumber);

        for (int i = 0; i < GroupNumber; ++i)
        {
            AgentGroups[i] = new List<GameObject>();

            int numOfMembers = Random.Range(2, MaxGroupMembers);
            for (int j = 0; j < numOfMembers; ++j)
            {
                MembersToCreate.Add(i);
            }

        }

        MembersToCreate = Utilities.FisherYatesSuffle<int>(MembersToCreate);
    }

    public void ReformGroups()
    {
        HashSet<int> deletedGroups = new HashSet<int>();

        for (int i = 0; i < GroupNumber; ++i)
        {
            if(AgentGroups[i].Count == 1)
            {
                for(int j = 0; j < GroupNumber; ++j)
                {
                    if(i != j && !deletedGroups.Contains(j) && AgentGroups[i].Count < MaxGroupMembers)
                    {
                        GameObject agent = AgentGroups[i][0];
                        agent.GetComponent<KinematicEntity>().SetNewGroup(j);
                        AgentGroups[j].Add(agent);
                        deletedGroups.Add(i);
                        break;
                    }
                }
            }
        }
        for(int i = 0; i < GroupNumber; ++i)
        {
            if(deletedGroups.Contains(i))
            {
                AgentGroups.Remove(i);
            }
        }
    }


}
