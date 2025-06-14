using UnityEngine;

public class FireShell : MonoBehaviour
{
    [SerializeField] private GameObject _bullet;
    [SerializeField] private GameObject _enemy;
    [SerializeField] private Transform _turretPos;
    [SerializeField] private Transform _turretBase;

    private float _rotationSpeed = 5f;
    private float _bulletSpeed = 15f;
    private float _moveSpeed = 1f;

    private float _fireDelay = 0.2f;
    private float _fireTimer = 0f;

    private void Update()
    {
        _fireTimer -= Time.deltaTime;

        Vector3 direction = (_enemy.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);

        float? angle = RotateTurret();

        if (angle != null && _fireTimer <= 0f)
        {
            Fire();
            _fireTimer = _fireDelay;
        }
        else
        {
            transform.Translate(0, 0, _moveSpeed * Time.deltaTime);
        }
    }

    private void Fire()
    {
        /* // for calculate trajectory method (old)
        Vector3 aimAt = CalculateTrajectory();
        if (aimAt != Vector3.zero)
        {
            transform.forward = aimAt;
            Instantiate(_bullet, _firePos.position, Quaternion.LookRotation(aimAt));
        }
        */

        // this is for the second way
        GameObject shell = Instantiate(_bullet, _turretPos.position, _turretPos.transform.rotation);
        shell.GetComponent<Rigidbody>().linearVelocity = _bulletSpeed * _turretBase.forward;   
    }

    // this is for the second way
    private float? RotateTurret()
    {
        float? angle = CalculateAngle(false);

        if (angle != null) 
        {
            _turretBase.localEulerAngles = new Vector3(360f - (float)angle, 0f, 0f);
        }
        return angle;
    }

    // this is for the second way
    private float? CalculateAngle(bool low)
    {
        Vector3 targetDir = _enemy.transform.position - transform.position; 
        float y = targetDir.y;
        targetDir.y = 0f;

        float x = targetDir.magnitude - 1; // added as hardcoded - 1 for more accurate shooting 
        float gravity = 9.8f;

        float speedSqr = _bulletSpeed * _bulletSpeed;
        float underTheSqrRoot = (speedSqr * speedSqr - gravity * (gravity * x * x + 2 * y * speedSqr));

        if (underTheSqrRoot >= 0f)
        {
            float root = Mathf.Sqrt(underTheSqrRoot);
            float highAngle = speedSqr + root;
            float lowAngle = speedSqr - root;

            if (low)
                return (Mathf.Atan2(lowAngle, gravity * x) * Mathf.Rad2Deg);
            else
                return (Mathf.Atan2(highAngle, gravity * x) * Mathf.Rad2Deg);
        }

        else
            return null;
    }

    // calculate time for when will be bullet and enemy in the same spot at the same time
    private Vector3 CalculateTrajectory()
    {
        // p for position, v for velocity, s for speed
        Vector3 p = _enemy.transform.position - transform.position;
        Vector3 v = _enemy.transform.forward * _enemy.GetComponent<DriveFutureLocations>().speed;
        float s = _bullet.GetComponent<Shell>().speed;

        // we want to find the point (time) for when they'll be in the same spot,
        // so that's an equation and we'll use the roots 
        // the equation is -> p.p + 2(p.v)t + (v.v - s.s), we use (-b +/- (sqrt(b.b -4a.c))/2.a
        float a = Vector3.Dot(v, v) - s * s;
        float b = 2 * Vector3.Dot(p, v);
        float c = Vector3.Dot(p, p);

        float d = b * b - 4 * a * c;

        if (d < 0.1f)
        {
            Debug.Log("Smaller than 0.1f");
            return Vector3.zero;
        }

        float sqrt = Mathf.Sqrt(d);
        float t1 = (-b - sqrt) / (2 * a);
        float t2 = (-b + sqrt) / (2 * a);

        float t = 0.0f;

        if (t1 < 0.0f && t2 < 0.0f)
        {
            Debug.Log("Both value is negative");
            return Vector3.zero;
        }
        else if (t1 < 0.0f) t = t2;
        else if (t2 < 0.0f) t = t1;
        else t = Mathf.Min(t1, t2);

        return p + t * v;

        // also you can create a static method for math operations because Unity doesnt have the all operations as method
    }
}
