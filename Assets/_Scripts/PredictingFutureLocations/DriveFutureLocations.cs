using UnityEngine;

public class DriveFutureLocations : MonoBehaviour
{
    public float speed = 6f;
    public float rotationSpeed = 100.0f;
    public Transform gun;
    public Transform firePos;
    public GameObject bullet;

    void Update()
    {
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        // Make it move 10 meters per second instead of 10 meters per frame...
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        // Move translation along the object's z-axis
        transform.Translate(0, 0, translation);

        // Rotate around our y-axis
        transform.Rotate(0, rotation, 0);

        HandleFire();
        HandleRotate();
    }

    private void HandleFire()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Instantiate(bullet, firePos.position, gun.rotation);
        }
    }

    private void HandleRotate()
    {
        if (Input.GetKey(KeyCode.T))
        {
            gun.RotateAround(gun.position, gun.right, -2);
        }
        else if (Input.GetKey(KeyCode.G))
        {
            gun.RotateAround(gun.position, gun.right, 2);
        }
    }
}
