using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubGoal
{
    //  A set of desired states that the agent wants to achieve. Example: "isHealed": 1
    public Dictionary<string, int> subGoals;

    // A flag to check if this goal should be removed after being achieved.
    // Some goals are one-time (e.g., get treated), others are repeatable (e.g., rest).
    public bool remove;

    // The constructor initializes the dictionary with one key-value pair (the goal)
    // and sets the remove flag.
    public SubGoal(string key, int value, bool remove)
    {
        subGoals = new Dictionary<string, int>();
        subGoals.Add(key, value);
        this.remove = remove;
    }
}

public class GAgent : MonoBehaviour
{
    // All available actions that this agent can perform.
    public List<GAction> actions = new List<GAction>();

    // All the agent's goals, each with a priority (int). Higher values mean more important.
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();

    // Holds the planned actions in order.
    private Queue<GAction> actionQueue;

    // The goal the agent is currently trying to achieve.
    private SubGoal currentGoal;

    // The action currently being performed.
    public GAction currentAction;

    private GPlanner planner;

    // refers to a collection of items or world states the agent "owns" or "holds".
    // It's used to track things the agent has picked up, gained, or needs in order to execute certain actions.
    public GInventory inventory = new GInventory();

    // agent’s personal knowledge or assumptions about the world.
    // If the agent believes "hasKey" = 1, it can plan to open a door.
    // Actions can modify beliefs via afterEffectsDict (e.g., setting "hasMedicine" = 1).
    public WorldStates beliefs = new WorldStates();

    private bool invoked = false;

    // use for some agent's destination point
    private Vector3 destination = Vector3.zero;

    public void Start()
    {
        // agent automatically finds and registers all GAction components attached to the GameObject.
        GAction[] acts = GetComponents<GAction>();

        foreach (GAction act in acts)
        {
            actions.Add(act);
        }
    }

    private void LateUpdate()
    {
        if (currentAction != null && currentAction.running)
        {
            float distanceToTarget = Vector3.Distance(destination, transform.position);
            if (distanceToTarget < 2f)
            {
                Debug.Log("Distance to Goal: " + currentAction.agent.remainingDistance);
                if (!invoked)
                {
                    Invoke(nameof(CompleteAction), currentAction.duration);
                    invoked = true;
                }
            }
            return;
        }

        if (planner == null || actionQueue == null)
        {
            planner = new GPlanner();

            var sortedGoals = from entry in goals orderby entry.Value descending select entry;

            foreach (KeyValuePair<SubGoal, int> sortedGoal in sortedGoals)
            {
                actionQueue = planner.CreatePlan(actions, sortedGoal.Key.subGoals, beliefs);

                if (actionQueue != null)
                {
                    currentGoal = sortedGoal.Key;
                    break;
                }
            }
        }

        if (actionQueue != null && actionQueue.Count == 0)
        {
            if (currentGoal.remove)
            {
                goals.Remove(currentGoal);
            }

            planner = null;
        }

        if (actionQueue != null && actionQueue.Count > 0)
        {
            currentAction = actionQueue.Dequeue();

            if (currentAction.PrePerform())
            {
                if (currentAction.target == null && currentAction.targetTag != string.Empty)
                {
                    currentAction.target = GameObject.FindWithTag(currentAction.targetTag);
                }

                if (currentAction.target != null)
                {
                    currentAction.running = true;

                    destination = currentAction.target.transform.position;

                    Transform dest = currentAction.target.transform.Find("Destination");

                    if (dest != null)
                    {
                        destination = dest.position;
                    }

                    currentAction.agent.SetDestination(destination);
                }
            }
            else
            {
                actionQueue = null;
            }
        }
    }

    private void CompleteAction()
    {
        currentAction.running = false;
        currentAction.PostPerform();
        invoked = false;
    }
}
