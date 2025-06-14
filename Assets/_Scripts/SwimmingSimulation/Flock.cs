using UnityEngine;

public class Flock : MonoBehaviour
{
    private float speed;
    private bool shoudlTurn = false; // when the fish reachs to its limit pos it'll turn

    private void Start()
    {
        speed = Random.Range(FlockingManager.Instance.minSpeed, FlockingManager.Instance.maxSpeed);
    }

    private void Update()
    {
        // unity method that represents limits of box/another collider
        // we create boundaries multiply by 2 because we want to ensure that fishes will be in swim limits 
        Bounds bounds = new Bounds(FlockingManager.Instance.transform.position, FlockingManager.Instance.swimLimits * 2);

        if (!bounds.Contains(transform.position))
        {
            shoudlTurn = true;
        }
        else
        {
            shoudlTurn = false;
        }

        if (shoudlTurn)
        {
            Vector3 direction = FlockingManager.Instance.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction),
                                                  FlockingManager.Instance.rotationSpeed * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0, 100) < 10)
            {
                speed = Random.Range(FlockingManager.Instance.minSpeed, FlockingManager.Instance.maxSpeed);
            }

            if (Random.Range(0, 100) < 10)
            {
                ApplyRules();
            }

            transform.Translate(0, 0, speed * Time.deltaTime);
        }
    }

    private void ApplyRules()
    {
        GameObject[] gos;
        gos = FlockingManager.Instance.allFish;

        Vector3 vectorToCenter = Vector3.zero;
        Vector3 vectorToAvoid = Vector3.zero;
        float groupSpeed = 0.01f;
        float neighbourDistance;
        int groupSize = 0;

        foreach (GameObject go in gos)
        {
            if (go != gameObject)
            {
                neighbourDistance = Vector3.Distance(go.transform.position, transform.position);

                if (neighbourDistance <= FlockingManager.Instance.neighbourDistance)
                {
                    vectorToCenter += go.transform.position;
                    groupSize++;

                    if (neighbourDistance < 1f)
                    {
                        vectorToAvoid += transform.position - go.transform.position;
                    }

                    Flock anotherFlock = go.GetComponent<Flock>();
                    groupSpeed += anotherFlock.speed;
                }
            }
        }

        if (groupSize > 0)
        {
            vectorToCenter = vectorToCenter / groupSize + (FlockingManager.Instance.goalPos - transform.position);
            speed = groupSpeed / groupSize;

            if (speed > FlockingManager.Instance.maxSpeed)
            {
                speed = FlockingManager.Instance.maxSpeed;
            }

            Vector3 direction = (vectorToCenter + vectorToAvoid) - transform.position;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, 
                                                      Quaternion.LookRotation(direction),
                                                      FlockingManager.Instance.rotationSpeed * Time.deltaTime);
            }
        }
    }
}
