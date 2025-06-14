using UnityEngine;

public class GStateMonitor : MonoBehaviour
{
    public string state; // The name of the state to monitor.
    public float stateDuration; // The duration for which the state should be monitored.
    public float stateDownRate; // The rate at which the state should be decreased.

    public WorldStates beliefs;
    public GameObject resourcePrefab;

    public string queueName;
    public string worldState;
    public GAction action;

    private bool stateFound = false;
    private float initialDuration;

    private void Awake()
    {
        beliefs = GetComponent<GAgent>().beliefs;
        initialDuration = stateDuration;
    }
    
    private void LateUpdate()
    {
        if (action.running)
        {
            stateFound = false;
            stateDuration = initialDuration;
        }

        if (!stateFound && beliefs.HasState(state))
        {
            stateFound = true;
        }
        
        if (stateFound)
        {
            stateDuration -= stateDownRate * Time.deltaTime;
            if (stateDuration <= 0)
            {
                Vector3 location = new Vector3(transform.position.x, resourcePrefab.transform.position.y, transform.position.z);
                GameObject resource = Instantiate(resourcePrefab, location, Quaternion.identity);

                stateFound = false;
                stateDuration = initialDuration;
                
                beliefs.RemoveState(state);

                GWorld.Instance.GetQueue(queueName).AddResource(resource);
                GWorld.Instance.GetWorld().ModifyState(worldState, 1);
            }
        }
    }
}