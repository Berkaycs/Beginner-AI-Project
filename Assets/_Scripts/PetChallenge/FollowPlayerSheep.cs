using System.Collections;
using UnityEngine;

public class FollowPlayerSheep : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private AudioSource _walkSource;
    [SerializeField] private AudioClip _walkClip;

    private float _speed = 5f;
    private float _rotationSpeed = 0.2f;
    private bool _isPlayingStepSound = false;

    private void Update()
    {
        AutoPilot();
    }

    private float GetCalculatedDistance()
    {
        // return Vector3.Distance(transform.position, _target.transform.position);

        Vector3 player = _target.transform.position;
        Vector3 sheep = transform.position;

        float distance = Vector3.Distance(sheep, player);

        return distance;
    }

    private void CalculateAngle()
    {
        Vector3 sheepForward = transform.forward;
        Vector3 playerDirection = _target.transform.position - transform.position;

        Debug.DrawRay(transform.position, sheepForward * 10, Color.green, 5);
        Debug.DrawRay(transform.position, playerDirection, Color.red, 5);

        float angle = Vector3.Angle(sheepForward, playerDirection);

        int clockwise = 1;

        if (Vector3.Cross(sheepForward, playerDirection).y < 0)
        {
            clockwise = -1;
        }

        if (angle > 10)
        {
            transform.Rotate(0, angle * clockwise * _rotationSpeed, 0);
        }
    }

    private void AutoPilot()
    {
        CalculateAngle();

        if (GetCalculatedDistance() > 5)
        {
            transform.position += transform.forward.normalized * _speed * Time.deltaTime;

            if (!_isPlayingStepSound)
            {
                StartCoroutine(PlayWalkSFX());
            }
        }
    }

    private IEnumerator PlayWalkSFX()
    {
        _isPlayingStepSound = true;
        _walkSource.PlayOneShot(_walkClip);
        yield return new WaitForSeconds(1); // Wait before allowing the next step sound
        _isPlayingStepSound = false;
    }
}
