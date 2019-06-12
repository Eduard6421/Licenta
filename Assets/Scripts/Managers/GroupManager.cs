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

    /// <summary>
    /// Check if groups with only 1 members exit
    /// If Yes then merge them with another group
    /// </summary>
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

    /// <summary>
    /// Create a group distribution for the agents
    /// </summary>
    public void CreateGroupDistribution()
    {

        CurrentGroup = 0;

        if(MaxGroupNumber == 0 )
        {
            BehaviourProbabilities.StopGroupDistributon();
            return;
        }

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
    

    /// <summary>
    /// Get next group for the given agent. Saves the agent in the specialised structure
    /// </summary>
    /// <param name="agent"> Agent that needs to be assigned</param>
    /// <returns></returns>
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
    /// <summary>
    /// Returns the numbers of a group with ca given groupID
    /// </summary>
    /// <param name="groupID"></param>
    /// <returns></returns>
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
