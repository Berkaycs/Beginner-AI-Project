using UnityEngine;

public class AIController : MonoBehaviour
{
    public Wheel[] wheels;
    public AntiRoll antiRollRef;
    public Circuit circuit;
    public AvoidDetector avoidDetector;
    public Rigidbody rb;
    public GameObject brakeLight;

    [Header("Car Settings")]
    public float lookAhead = 40f;
    public float steeringSensitivity = 0.01f;
    public float antiRollValue = 5000f;
    public float maxTorque = 200f;
    public float maxBrakeTorque = 500f;
    public float maxSteerAngle = 60f;
    public float accelCornerMax = 20f;
    public float brakeCornerMax = 10f;
    public float accelVelocityThreshold = 20f;
    public float brakeVelocityThreshold = 10f;
    public float trackerSpeed = 15f;

    private Vector3 target;
    private int currentWP = 0;

    private GameObject tracker;
    private int currentTrackerWP = 0;


    private void Start()
    {
        target = circuit.waypoints[currentWP].transform.position;

        tracker = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        Destroy(tracker.GetComponent<Collider>());
        //tracker.GetComponent<MeshRenderer>().enabled = false;
        tracker.transform.position = transform.position;
        tracker.transform.rotation = transform.rotation;

        antiRollRef.antiRoll = antiRollValue;

        foreach (Wheel wheel in wheels)
        {
            wheel.maxTorque = maxTorque;
            wheel.maxSteerAngle = maxSteerAngle;
            wheel.maxBrakeTorque = maxBrakeTorque;
        }
    }

    private void Update()
    {
        ProgressTracker();
        target = tracker.transform.position;

        Vector3 localTarget;

        if (Time.time < avoidDetector.avoidTime)
        {
            localTarget = tracker.transform.right * avoidDetector.avoidPath;
        }
        else
        {
            localTarget = transform.InverseTransformPoint(target);
        }

        float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

        // speed
        float s = Mathf.Clamp(targetAngle * steeringSensitivity, -1, 1) * Mathf.Sign(rb.linearVelocity.magnitude);

        float corner = Mathf.Clamp(Mathf.Abs(targetAngle), 0, 90);
        float cornerNormalized = corner / 90f;

        // static acceleration
        float a = 1f; // max value 1 because of normalization 

        if (corner > accelCornerMax && rb.linearVelocity.magnitude > accelVelocityThreshold)
            a = Mathf.Lerp(0, 1, 1 - cornerNormalized);

        // brake
        float b = 0;

        if (corner > brakeCornerMax && rb.linearVelocity.magnitude > brakeVelocityThreshold)
            b = Mathf.Lerp(0, 1, cornerNormalized);

        if (avoidDetector.reverse)
        {
            a = -1 * a;
            s = -1 * s;
        }

        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].Go(a, s, b);
        }

        if (b > 0)
        {
            brakeLight.SetActive(true);
        }
        else
        {
            brakeLight.SetActive(false);
        }
    }

    private void ProgressTracker()
    {
        Debug.DrawLine(transform.position, tracker.transform.position);

        if (Vector3.Distance(transform.position, tracker.transform.position) > lookAhead)
        {
            trackerSpeed -= 1f;
            if (trackerSpeed < 2f) trackerSpeed = 2f;
            return;
        }

        if (Vector3.Distance(transform.position, tracker.transform.position) < lookAhead / 2f)
        {
            trackerSpeed += 1f;
            if (trackerSpeed > 15f) trackerSpeed = 15f;
        }

        tracker.transform.LookAt(circuit.waypoints[currentTrackerWP].transform.position);
        tracker.transform.Translate(0, 0, trackerSpeed * Time.deltaTime);

        if (Vector3.Distance(tracker.transform.position,
                             circuit.waypoints[currentTrackerWP].transform.position) < 1)
        {
            currentTrackerWP++;
            
            if (currentTrackerWP >= circuit.waypoints.Length)
                currentTrackerWP = 0;
        }
    }
}
