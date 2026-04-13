using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private Camera _cam;
    public float moveSpeed = 10f;
    public float rotationSpeed = 20f;

    private void Start()
    {
        _cam = GetComponentInChildren<Camera>();
        _cam.gameObject.transform.LookAt(transform.position);
    }

    private void Update()
    {
        float translation = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float translation2 = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

        transform.Translate(0, 0, translation);
        transform.Translate(translation2, 0, 0);

        if (Input.GetKey(KeyCode.Z))
            transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0);

        if (Input.GetKey(KeyCode.C))
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        if (Input.GetKey(KeyCode.R) && _cam.transform.position.y > 5)
            _cam.gameObject.transform.Translate(0, 0, moveSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.F) && _cam.transform.position.y < 45)
            _cam.gameObject.transform.Translate(0, 0, -moveSpeed * Time.deltaTime);

        float angle = Vector3.Angle(_cam.transform.forward, Vector3.up);
        //Debug.Log(angle);

        if (Input.GetKey(KeyCode.T) && angle < 175)
            _cam.gameObject.transform.Translate(Vector3.up);
            _cam.gameObject.transform.LookAt(transform.position);

        if (Input.GetKey(KeyCode.G) && angle > 95)
            _cam.gameObject.transform.Translate(-Vector3.up);
            _cam.gameObject.transform.LookAt(transform.position);
    }
}
