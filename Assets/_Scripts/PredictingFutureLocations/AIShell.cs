using UnityEngine;

public class AIShell : MonoBehaviour
{
    public GameObject explosion;
    public Rigidbody rb;

    void Update()
    {
        transform.forward = rb.linearVelocity;
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
