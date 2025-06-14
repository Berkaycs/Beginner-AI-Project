using System.Collections.Generic;
using UnityEngine;

public class GNode
{
    // This points to the previous node in the plan tree.
    // Used for backtracking the sequence of actions
    // from the final node to the root when a valid plan is found.
    public GNode parent;

    // The cumulative cost to reach this node from the starting node.
    // Helps the planner choose the least costly path when multiple plans are possible.
    public float cost;

    // The world state at this node — a dictionary of key-value pairs
    // like "isTreated": 1 or "isResting": 0.
    // Represents what the world looks like after applying the action from the parent node.
    public Dictionary<string, int> state;

    // The GAction object that was executed to get to this node.
    // This is what changes the world state from the parent node's state to this node’s state.
    public GAction action;

    public GNode(GNode parent, float cost, Dictionary<string, int> allStates, GAction action)
    {    
        this.parent = parent;       // Setting the parent.
        this.cost = cost;           //Assigning the total cost.
        state = new Dictionary<string, int>(allStates); // Copying the current world state to avoid unwanted shared references.
        this.action = action;       // Linking the action that was just taken.
    }

    public GNode(GNode parent, float cost, Dictionary<string, int> allStates, Dictionary<string, int> beliefStates, GAction action)
    {
        this.parent = parent;       // Setting the parent.
        this.cost = cost;           //Assigning the total cost.
        state = new Dictionary<string, int>(allStates); // Copying the current world state to avoid unwanted shared references.

        foreach(KeyValuePair<string, int> beliefState in beliefStates)
        {
            if (!state.ContainsKey(beliefState.Key))
            {
                state.Add(beliefState.Key, beliefState.Value);
            }
        }

        this.action = action;       // Linking the action that was just taken.
    }
}

// The GPlanner builds a tree of possible actions from the current world state
// and tries to find the cheapest path (lowest cost) that achieves the desired goal.
// If such a path exists, it returns the plan as a Queue<GAction>.
public class GPlanner
{
    // PARAMETERS
    // actions -> List of all possible actions the agent can take.
    // goal: The desired world state (e.g., { "hasFood": 1 }).
    // states: The current state of the world.
    public Queue<GAction> CreatePlan(List<GAction> actions, Dictionary<string, int> goal, WorldStates beliefStates)
    {
        List<GAction> usableActions = new List<GAction>();

        foreach (GAction action in actions)
        {
            if (action.IsAchievable())
            {
                usableActions.Add(action);
            }
        }

        List<GNode> leaves = new List<GNode>();

        // no parent, no cost, no action
        GNode firstNode = new GNode(null, 0, GWorld.Instance.GetWorld().GetStates(), beliefStates.GetStates(), null);

        // recursively builds all possible action sequences.
        // adds the nodes that successfully reach the goal into leaves.
        bool success = BuildGraph(firstNode, leaves, usableActions, goal);

        if (!success)
        {
            Debug.Log("NO PLAN");
            return null;
        }

        GNode cheapest = null;

        // Among all successful goal-reaching paths, choose the one with the lowest total cost.
        foreach (GNode leaf in leaves)
        {
            if (cheapest == null) 
                cheapest = leaf;
            else
                if (leaf.cost < cheapest.cost) 
                    cheapest = leaf;
        }

        // Backtrack from the goal node to the root node and build the list of actions.
        List<GAction> result = new List<GAction>();
        GNode node = cheapest;


        // Start from the goal node (cheapest node).
        // Keep walking up the tree until you reach the root node (which has parent == null).
        while (node != null)
        {
            // The root node (first node created in the planner) doesn't represent a real action; it’s just the starting state.
            // So you skip adding null actions from the root node.
            // Only actual GActions (real steps the agent took) are added to the result.
            if (node.action != null)
            {
                result.Insert(0, node.action);
                /*
                 Let's say:

                node1 (goal) has action = Eat

                node2 (parent) has action = Cook

                node3 (parent) has action = Gather

                As we walk backwards:

                Insert(0, Eat) -> result = [Eat]

                Insert(0, Cook) -> result = [Cook, Eat]

                Insert(0, Gather) -> result = [Gather, Cook, Eat]

                So now the plan is in the correct order: Gather -> Cook -> Eat */
            }
            node = node.parent;
            /*
                This moves one step up the tree.

                From goal node to the action before it,

                Eventually reaching the root (start of the plan).
             */
        }

        // Converts the list into a queue so actions can be executed in order.
        Queue<GAction> queue = new Queue<GAction>();
        foreach (GAction action in result)
        {
            queue.Enqueue(action);
        }

        Debug.Log("The plan is: ");

        foreach (GAction action in queue)
        {
            Debug.Log("Q: " + action.actionName);
        }

        return queue;
    }

    // recursive function that builds the tree of action sequences.
    private bool BuildGraph(GNode parent, List<GNode> leaves, List<GAction> usableActions, Dictionary<string, int> goal)
    {
        bool foundPath = false;

        foreach (GAction action in usableActions)
        {
            // Only consider actions whose preconditions are satisfied in the current state.
            if (action.IsAchievableGiven(parent.state))
            {
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);

                // Apply the action's effects to generate the new state.
                foreach (KeyValuePair<string, int> effect in action.afterEffectsDict)
                {
                    if (!currentState.ContainsKey(effect.Key))
                    {
                        currentState.Add(effect.Key, effect.Value);
                    }
                }

                GNode node = new GNode(parent, parent.cost + action.cost, currentState, action);

                // If the goal is achieved in the new state, store this node as a potential plan.
                if (IsGoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                // Otherwise, recursively continue building the graph using remaining actions (minus the one just used).
                else
                {
                    List<GAction> subset = ActionSubset(usableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goal);

                    if (found) 
                        foundPath = true;
                }
            }
        }

        return foundPath;
    }

    // Checks if the current state contains all goal keys
    private bool IsGoalAchieved(Dictionary<string, int> goals, Dictionary<string, int> state)
    {
        foreach (KeyValuePair<string, int> goal in goals)
        {
            if (!state.ContainsKey(goal.Key))
                return false;
        }
        return true;
    }

    // Removes the current action from the list, so it doesn’t get used again in this path 
    private List<GAction> ActionSubset(List<GAction> actions, GAction removeMe)
    {
        List<GAction> subset = new List<GAction>();

        foreach (GAction action in actions)
        {
            if (!action.Equals(removeMe))
                subset.Add(action);
        }
        return subset;
    }
}
