using System.Collections.Generic;

[System.Serializable]
public class WorldState
{
    // A simple key-value pair, like "isHealed", 1
    public string key;
    public int value;
}

public class WorldStates
{
    // Holds all world states in a dictionary.
    public Dictionary<string, int> states;

    public WorldStates()
    {
        states = new Dictionary<string, int>();
    }

    // Checks if a state exists.
    public bool HasState(string key)
    {
        return states.ContainsKey(key);
    }

    private void AddState(string key, int value)
    {
        states.Add(key, value);
    }

    // Adds or updates the value. Removes state if it drops to zero or below.
    public void ModifyState(string key, int value)
    {
        if (states.ContainsKey(key))
        {
            states[key] += value;

            if (states[key] <= 0)
            {
                RemoveState(key);
            }
        }
        else
        {
            states.Add(key, value);
        }
    }

    public void RemoveState(string key)
    {
        if (states.ContainsKey(key))
        {
            states.Remove(key);
        }
    }

    public void SetState(string key, int value)
    {
        if (states.ContainsKey(key))
        {
            states[key] = value;
        }
        else
        {
            states.Add(key, value);
        }
    }

    public Dictionary<string, int> GetStates()
    {
        return states;
    }
}
