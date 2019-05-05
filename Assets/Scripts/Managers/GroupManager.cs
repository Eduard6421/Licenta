using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroupManager : MonoBehaviour
{
    private static GroupManager instance;

    private GoalManager GoalMaster;

    private int CurrentGroup;


    [SerializeField]
    private int MaxGroupNumber = 1;
    private int GroupNumber;

    [SerializeField]
    private int MaxGroupMembers = 2;
    private Dictionary<int,List<GameObject>> AgentGroups;

    private Dictionary<int,int> CurrentAgents;
    private Dictionary<int, Utilities.Actions> GroupActions;

    private List<int> MembersToCreate;

    public static GroupManager GetInstance()
    {
        return instance;    
    }

    void ReformGroups()
    {
        HashSet<int> deletedGroups = new HashSet<int>();

        for (int i = 0; i < GroupNumber; ++i)
        {
            if (AgentGroups[i].Count == 1)
            {
                for (int j = 0; j < GroupNumber; ++j)
                {
                    if (i != j && !deletedGroups.Contains(j) && AgentGroups[i].Count < MaxGroupMembers)
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
        for (int i = 0; i < GroupNumber; ++i)
        {
            if (deletedGroups.Contains(i))
            {
                AgentGroups.Remove(i);
            }
        }

        // Set each dictionary to the number of members in a group and 
        // Save how many current agents finished their temporary task

        foreach (KeyValuePair<int, List<GameObject>> entry in AgentGroups)
        {
            CurrentAgents[entry.Key] = 0;
            GroupActions[entry.Key] = Utilities.Actions.Nothing;
        }
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

        AgentGroups = new Dictionary<int, List<GameObject>>();
        CurrentAgents = new Dictionary<int, int>();
        GroupActions = new Dictionary<int, Utilities.Actions>();
        MembersToCreate = new List<int>();
    }

    void Start()
    {
        GoalMaster = GoalManager.GetInstance();
    }

    public void CreateGroupDistribution()
    {

        CurrentGroup = 0;
        GroupNumber = Random.Range(1, MaxGroupNumber);

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
        ReformGroups();
    }

    public void SetGroupAction(int groupID, Utilities.Actions groupAction, Goal groupGoal)
    {
        GroupActions[groupID] = groupAction;
        CurrentAgents[groupID] = 0;
    }
    
    public int GetNextGroup(GameObject agent)
    {
        int groupIndex;

        if (MembersToCreate.Count > 0)
        {
            if (this.CurrentGroup == MembersToCreate.Count - 1)
            {
                BehaviourProbabilities.StopGroupDistributon();
            }

            groupIndex = MembersToCreate[this.CurrentGroup];
            AgentGroups[groupIndex].Add(agent);

            this.CurrentGroup++;

            return groupIndex;
        }

        BehaviourProbabilities.StopGroupDistributon();
        return -1;

    }

    public int GetGroupSize(int groupID)
    {
        return AgentGroups[groupID].Count;
    }

    public Utilities.Actions GetCurrentGroupAction(int groupID)
    {
        if (GroupActions[groupID] == Utilities.Actions.Nothing)
        {
            return GroupActions[groupID];
        }
        if (CurrentAgents[groupID] < AgentGroups[groupID].Count)
        {
            ++CurrentAgents[groupID];
            return Utilities.Actions.Wait;
        }
        else
        {
            CurrentAgents[groupID] = 0;
            return Utilities.Actions.Nothing;
        }
    }

    public List<GameObject> GetGroupAgents(int groupID)
    {
        return AgentGroups[groupID];
    }






}
