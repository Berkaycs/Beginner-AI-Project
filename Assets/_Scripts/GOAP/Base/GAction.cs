using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class GAction : MonoBehaviour
{
    public string actionName = "Action";    // default action name
    public float cost = 1f;         // weight of action
    public float duration = 0f;     // duration of action

    public GameObject target;       // target location of action
    public string targetTag;
    public NavMeshAgent agent;

    public WorldState[] preConditions; // World states that must be true before this action can happen.
    public WorldState[] afterEffects; // World states that will be true after the action.

    // dictionary versions for faster lookup during planning.
    public Dictionary<string, int> preConditionsDict = new Dictionary<string, int>();
    public Dictionary<string, int> afterEffectsDict = new Dictionary<string, int>();

    public GInventory inventory;        

    public WorldStates beliefs;   // agent’s understanding of the world.

    public bool running = false;  // Whether the action is currently being performed.

    public void Awake()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();

        // Converts preConditions and afterEffects arrays into dictionaries for planning.
        if (preConditions != null)
        {
            foreach (WorldState condition in preConditions)
            {
                preConditionsDict.Add(condition.key, condition.value);
            }
        }

        if (afterEffects != null)
        {
            foreach (WorldState effect in afterEffects)
            {
                afterEffectsDict.Add(effect.key, effect.value);
            }
        }

        inventory = GetComponent<GAgent>().inventory;

        beliefs = GetComponent<GAgent>().beliefs;
    }

    public bool IsAchievable()
    {
        // Every action is currently considered achievable.
        return true;
    }

    // Checks if this action can run given a set of world conditions.
    public bool IsAchievableGiven(Dictionary<string, int> conditions)
    {
        foreach (KeyValuePair<string, int> preCondition in preConditionsDict)
        {
            if (!conditions.ContainsKey(preCondition.Key)) return false;
        }
        return true;
    }

    public abstract bool PrePerform(); // Called before the action starts. Used for setup or checking further conditions.
    public abstract bool PostPerform(); // Called after the action finishes. Used to apply effects or cleanup.
}
