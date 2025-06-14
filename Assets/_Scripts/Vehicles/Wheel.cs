using UnityEngine;

public class Wheel : MonoBehaviour
{
    public WheelCollider wheelCollider;
    public GameObject wheelMesh;

    public float maxTorque = 200f; // rotational force that is calculated using pivot point
    public float maxBrakeTorque = 500f;
    public float maxSteerAngle = 60f;

    public bool canTurn = false;

    public void Go(float accel, float steer, float brake)
    {
        // apply force and rotate colliders 
        accel = Mathf.Clamp(accel, -1, 1);

        float thrustTorque = accel * maxTorque;

        wheelCollider.motorTorque = thrustTorque;

        if (canTurn)
        {
            steer = Mathf.Clamp(steer, -1, 1) * maxSteerAngle;
            wheelCollider.steerAngle = steer;
        }
        else
        {
            brake = Mathf.Clamp(brake, -1, 1) * maxBrakeTorque;
            wheelCollider.brakeTorque = brake;
        }

            // rotating meshes
            Quaternion quat;
        Vector3 position;

        wheelCollider.GetWorldPose(out position, out quat);
        wheelMesh.transform.position = position;
        wheelMesh.transform.rotation = quat;
    }
}
