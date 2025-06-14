using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;

public class DriveDistance : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _rotationSpeed = 100f;
    [SerializeField] private GameObject _fuel;

    // Challenge
    private bool _hasAutopilot = false;
    private float _tSpeed = 2f;
    private float _tRotationSpeed = 0.1f;

    private void LateUpdate()
    {
        float translation = Input.GetAxis("Vertical") * _speed * Time.deltaTime;
        float rotation = Input.GetAxis("Horizontal") * _rotationSpeed * Time.deltaTime;

        transform.Translate(0, translation, 0);
        transform.Rotate(0, 0, -rotation);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetCalculatedDistanceAsVector();
            CalculateAngle();
        }

        // Challenge 
        if (Input.GetKeyDown(KeyCode.T))
        {
            _hasAutopilot = !_hasAutopilot;
        }

        if (GetCalculatedDistanceAsFloat() < 4)
        {
            _hasAutopilot = false;
        }

        if (_hasAutopilot)
        {
            AutoPilot();
        }
    }

    private float GetCalculatedDistanceAsFloat()
    {
        Vector3 tankPos = transform.position;
        Vector3 fuelPos = _fuel.transform.position;

        // With Pythogoras Theorem
        float distance = Mathf.Sqrt(Mathf.Pow(fuelPos.x - tankPos.x, 2) +
                                    Mathf.Pow(fuelPos.y - tankPos.y, 2));

        // With Unity Method
        float uDistance = Vector2.Distance(tankPos, fuelPos);

        // With magnitude
        Vector2 tankToFuel = fuelPos - tankPos;

        Debug.Log("Distance: " + distance);
        Debug.Log("Unity Distance: " + uDistance);
        Debug.Log("Vector Magnitude: " + tankToFuel.magnitude);
        // using sqrMagnitude more performant mostly especially in update method
        Debug.Log("Vector SqMagnitude: " + tankToFuel.sqrMagnitude); 

        return distance;
    }

    private Vector3 GetCalculatedDistanceAsVector()
    {
        Vector3 tankPos = transform.position;
        Vector3 fuelPos = _fuel.transform.position;

        // With Pythogoras Theorem
        float distance = Mathf.Sqrt(Mathf.Pow(fuelPos.x - tankPos.x, 2) +
                                    Mathf.Pow(fuelPos.y - tankPos.y, 2));

        // With Unity Method
        float uDistance = Vector2.Distance(tankPos, fuelPos);

        // With magnitude
        Vector3 tankToFuel = fuelPos - tankPos;

        Debug.Log("Distance: " + distance);
        Debug.Log("Unity Distance: " + uDistance);
        Debug.Log("Vector Magnitude: " + tankToFuel.magnitude);
        // using sqrMagnitude more performant mostly especially in update method
        Debug.Log("Vector SqMagnitude: " + tankToFuel.sqrMagnitude);

        return tankToFuel;
    }

    private void CalculateAngle()
    {
        Vector3 tankForward = transform.up;
        Vector3 fuelDirection = _fuel.transform.position - transform.position;

        // Forward length is normalized (1) so to see the line we make it bigger by multiply 10
        Debug.DrawRay(transform.position, tankForward * 10, Color.green, 5); 
        Debug.DrawRay(transform.position, fuelDirection, Color.red, 5);

        // Calculating Angle

        // With Mathematics
        float dot = tankForward.x * fuelDirection.x + tankForward.y * fuelDirection.y;
        float angle = Mathf.Acos(dot / (tankForward.magnitude * fuelDirection.magnitude)) * Mathf.Rad2Deg;

        // Default value is radiant type so we convert it to degree
        Debug.Log("Angle: " + angle);

        // With Unity Method
        Debug.Log("Unity Angle: " + Vector3.Angle(tankForward, fuelDirection));

        // Rotating object to the target
        int clockwise = 1; // should turn right or left direction

        if (Cross(tankForward, fuelDirection).z < 0)
        {
            clockwise = -1;
        }

        if (angle > 10)
        {
            transform.Rotate(0, 0, angle * clockwise * _tRotationSpeed);
        }
    }

    private Vector3 Cross(Vector3 a, Vector3 b)
    {
        float xMult = a.y * b.z - a.z * b.y;
        float yMult = a.x * b.z - a.z * b.x;
        float zMult = a.x * b.y - a.y * b.x;

        return new Vector3(xMult, yMult, zMult);
    }

    // Challenge 
    private void AutoPilot()
    {
        CalculateAngle();

        // it's move to forward of itself and when it's needed it turns 
        transform.position += transform.up.normalized * _tSpeed * Time.deltaTime;

        // it's directly turn and move to target 
        //Vector3 velocity = GetCalculatedDistanceAsVector().normalized * _tSpeed * Time.deltaTime;
        //transform.position += velocity;
    }
}
