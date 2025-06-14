using UnityEngine;

public class Shell : MonoBehaviour
{
    public GameObject explosion;
    public float speed = 6;
    public float mass = 10f;
    public float force = 4f;
    public float drag = 1f;
    public float acceleration;

    // gravity
    public float ySpeed;
    public float gravity = -9.8f; // default
    public float gravityAcceleration;

    private void Start()
    {
        // apply force in start
        acceleration = force / mass;
        speed += acceleration;

        // calculating gravity acceleration (deceleration)
        gravityAcceleration = gravity / mass;
    }

    private void LateUpdate()
    {
        // reduce speed with air drag while time passes
        speed *= (1 - Time.deltaTime * drag);

        // calculating speed on Y axis
        ySpeed += gravityAcceleration * Time.deltaTime;

        // move forward
        transform.Translate(0, ySpeed, Time.deltaTime * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "tank" || collision.gameObject.tag == "Ground")
        {
            GameObject exp = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(exp, 0.5f);
            Destroy(gameObject);
        }
    }
}
