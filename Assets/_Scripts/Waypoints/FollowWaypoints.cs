using UnityEngine;

public class FollowWaypoints : MonoBehaviour
{
    public GameObject[] waypoints;
    private GameObject tracker;
    public float speed = 10f;
    public float rotSpeed = 10f;
    public float lookAhead = 10f;
    public int currentWP = 0;

    private void Start()
    {
        tracker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Destroy(tracker.GetComponent<Collider>());
        tracker.GetComponent<MeshRenderer>().enabled = false;
        tracker.transform.position = transform.position;
        tracker.transform.rotation = transform.rotation;
    }

    private void Update()
    {
        ProgressTracker();
        FollowWP();
    }

    private void ProgressTracker()
    {
        // we use a tracker to prevent unstable behaviours of our tank
        // when they follow a static objects with wrong turn/distance values

        if (Vector3.Distance(tracker.transform.position, transform.position) > lookAhead) return;

        if (Vector3.Distance(tracker.transform.position, waypoints[currentWP].transform.position) < 3)
            currentWP++;

        if (currentWP >= waypoints.Length)
            currentWP = 0;

        tracker.transform.LookAt(waypoints[currentWP].transform.position);
        tracker.transform.Translate(0, 0, (speed + 2) * Time.deltaTime);
    }

    private void FollowWP()
    {
        //transform.LookAt(waypoints[currentWP].transform); instead of this we use slerp

        Quaternion lookAtWP = Quaternion.LookRotation(tracker.transform.position - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookAtWP, rotSpeed * Time.deltaTime);

        transform.Translate(0, 0, speed * Time.deltaTime);
    }
}
