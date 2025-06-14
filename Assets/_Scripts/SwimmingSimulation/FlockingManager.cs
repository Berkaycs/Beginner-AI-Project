using UnityEngine;

public class FlockingManager : MonoBehaviour
{
    public static FlockingManager Instance;

    public GameObject fishPrefab;
    public int numFish = 20;
    public GameObject[] allFish;
    public Vector3 swimLimits = new Vector3(5, 5, 5);
    public Vector3 goalPos = Vector3.zero;

    [Header("Fish Settings")]
    [Range(0f, 5f)]
    public float minSpeed;
    [Range(0f, 5f)]
    public float maxSpeed;
    [Range(1f, 10f)]
    public float neighbourDistance;
    [Range(1f, 5f)]
    public float rotationSpeed;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        allFish = new GameObject[numFish];

        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = transform.position + new Vector3(Random.Range(-swimLimits.x,swimLimits.x), 
                                                           Random.Range(-swimLimits.y,swimLimits.y), 
                                                           Random.Range(-swimLimits.z, swimLimits.z));

            allFish[i] = Instantiate(fishPrefab, pos, Quaternion.identity);
            goalPos = transform.position;
        }
    }

    private void Update()
    {
        if (Random.Range(0, 100) < 10)
        {
            goalPos = transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                               Random.Range(-swimLimits.y, swimLimits.y),
                                               Random.Range(-swimLimits.z, swimLimits.z));
        }
    }
}
