using UnityEngine;

public class Drive : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _rotationSpeed = 100f;

    private void Update()
    {
        float translation = Input.GetAxis("Vertical") * _speed * Time.deltaTime;
        float rotation = Input.GetAxis("Horizontal") * _rotationSpeed * Time.deltaTime;

        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);
    }
}
