using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceQueue
{
    public Queue<GameObject> queue = new Queue<GameObject>();
    public string tag;
    public string modState;

    public ResourceQueue(string tag, string modState, WorldStates world)
    {
        this.tag = tag;
        this.modState = modState;

        if (tag != "")
        {
            GameObject[] resources = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject resource in resources)
            {
                queue.Enqueue(resource);
            }
        }

        if (modState != "")
        {
            world.ModifyState(modState, queue.Count);
        }
    }

    public void AddResource(GameObject resource)
    {
        queue.Enqueue(resource);
    }

    public void RemoveResource(GameObject goToCheck)
    {
        // if the gameObject is not equal the one we select to destroy, reorder the queue without it
        queue = new Queue<GameObject>(queue.Where(go => go != goToCheck));
    }

    public GameObject RemoveResource()
    {
        if (queue.Count == 0) return null;

        return queue.Dequeue();
    }
}

public sealed class GWorld
{
    // used to access the global world state.
    private static readonly GWorld instance = new GWorld();

    // The global state of the environment.
    private static WorldStates world;

    private static ResourceQueue patientQueue;
    private static ResourceQueue cubicleQueue;
    private static ResourceQueue officesQueue;
    private static ResourceQueue toiletsQueue;
    private static ResourceQueue puddleQueue;

    private static Dictionary<string, ResourceQueue> resources = new Dictionary<string, ResourceQueue>();

    static GWorld()
    {
        world = new WorldStates();

        patientQueue = new ResourceQueue("", "", world); // when there are patients waiting to be treated it'll be added to the queue
        resources.Add("patient", patientQueue);

        cubicleQueue = new ResourceQueue("Cubicle", "FreeCubicle", world); // cubicles waiting to be used
        resources.Add("cubicles", cubicleQueue);

        officesQueue = new ResourceQueue("Office", "FreeOffice", world); // offices waiting to be used
        resources.Add("offices", officesQueue);

        toiletsQueue = new ResourceQueue("Toilet", "FreeToilet", world); // toilets waiting to be used
        resources.Add("toilets", toiletsQueue);

        puddleQueue = new ResourceQueue("Puddle", "FreePuddle", world); // puddles waiting to be cleaned
        resources.Add("puddles", puddleQueue);
    }

    public ResourceQueue GetQueue(string type)
    {
        return resources[type];
    }

    private GWorld()
    {

    }

    public static GWorld Instance
    {
        get { return instance; }
    }

    // Used to store facts like "isPatientWaiting": 1.
    public WorldStates GetWorld()
    {
        return world;
    }
}
