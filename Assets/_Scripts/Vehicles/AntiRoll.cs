using UnityEngine;

public class AntiRoll : MonoBehaviour
{
    public WheelCollider wheelLFront;
    public WheelCollider wheelRFront;
    public WheelCollider wheelRBack;
    public WheelCollider wheelLBack;
    public Rigidbody rb;
    public float antiRoll = 5000f;

    private void Update()
    {
        GroundWheels(wheelLFront, wheelRFront);
        GroundWheels(wheelLBack, wheelRBack);
    }

    private void GroundWheels(WheelCollider WL, WheelCollider WR)
    {
        WheelHit hit;
        float travelL = 1f;
        float travelR = 1f;

        bool groundedL = WL.GetGroundHit(out hit);
        if (groundedL)
        {
            travelL = (-WL.transform.InverseTransformPoint(hit.point).y - WL.radius) / WL.suspensionDistance;
        }

        bool groundedR = WR.GetGroundHit(out hit);
        if (groundedR)
        {
            travelR = (-WR.transform.InverseTransformPoint(hit.point).y - WR.radius) / WR.suspensionDistance;
        }

        float antiRollForce = (travelL - travelR) * antiRoll;

        if (groundedL)
        {
            rb.AddForceAtPosition(WL.transform.up * -antiRollForce, WL.transform.position);
        }

        if (groundedR)
        {
            rb.AddForceAtPosition(WR.transform.up * -antiRollForce, WR.transform.position);
        }
    }
}
