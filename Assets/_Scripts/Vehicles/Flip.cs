using UnityEngine;

public class Flip : MonoBehaviour
{
    public Rigidbody rb;
    public float lastTimeChecked;

    private void Update()
    {
        if (transform.up.y > 0.5f || rb.linearVelocity.magnitude > 1)
        {
            lastTimeChecked = Time.time;
        }

        // after 3 sec turning down, turn up the car
        if (Time.time > lastTimeChecked + 3)
        {
            RightCar();
        }
    }

    private void RightCar()
    {
        transform.position += Vector3.up; // turn up the car
        transform.rotation = Quaternion.LookRotation(transform.forward); // rotate the car to its forward
    }
}
