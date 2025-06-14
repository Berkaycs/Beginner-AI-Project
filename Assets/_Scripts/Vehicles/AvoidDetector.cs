using UnityEngine;

public class AvoidDetector : MonoBehaviour
{
    public Rigidbody rb;
    public float avoidPath = 0;
    public float avoidTime = 0;
    public float wanderDistance = 4;
    public float avoidLength = 0;
    public bool reverse = false;


    private void OnTriggerStay(Collider other)
    {
        Vector3 collisionDir = transform.InverseTransformPoint(other.transform.position);

        if (collisionDir.x > 0 && collisionDir.z > 0)
        {
            if (rb.linearVelocity.magnitude < 1) reverse = true;

            else if (other.gameObject.tag == "car")
            {
                Rigidbody otherCar = other.GetComponent<Rigidbody>();
                avoidTime = Time.deltaTime + avoidLength;

                Vector3 otherCarLocalTarget = transform.InverseTransformPoint(other.gameObject.transform.position);
                float otherCarAngle = Mathf.Atan2(otherCarLocalTarget.x, otherCarLocalTarget.z);
                avoidPath = wanderDistance * -Mathf.Sign(otherCarAngle);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        reverse = false;
        if (other.gameObject.tag != "car") return;
        avoidTime = 0;
    }
}
