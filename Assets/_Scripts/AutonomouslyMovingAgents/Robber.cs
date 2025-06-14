using UnityEngine;
using UnityEngine.AI;

public class Robber : MonoBehaviour
{
    public NavMeshAgent agent;
    public CopController target;

    private float wanderRadius = 10f; // the radius of the circle.
    private float wanderDistance = 10f; // how far in front of the agent the circle is.
    private float wanderJitter = 1f; // how much the point changes per frame (adds randomness).

    private Vector3 wanderTarget; // a moving target point on a circle in front of the agent.

    private bool coolDown = false;

    private void Update()
    {
        //Seek(target.position);
        //Flee(target.transform.position);
        //Pursue();
        //Evade();
        //Wander();
        //Hide();

        if (!coolDown)
        {
            if (!IsTargetInRange())
            {
                Wander();
            }
            else if (CanSeeTarget() && CanSeeMe())
            {
                CleverHide();
                coolDown = true;
                Invoke(nameof(BehaviourCooldown), 5);
            }
            else
            {
                Pursue();
            }
        }
    }

    // Move to a specific location
    private void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    // Move away from a threat
    private void Flee(Vector3 location)
    {
        Vector3 fleeVector = location - transform.position;
        agent.SetDestination(transform.position - fleeVector);
    }

    // Chase a moving target
    private void Pursue()
    {
        Vector3 targetDir = target.transform.position - transform.position;

        float relativeHeading = Vector3.Angle(transform.forward, transform.TransformVector(target.transform.forward));
        float toTarget = Vector3.Angle(transform.forward, transform.TransformVector(targetDir));

        if ((toTarget > 90 && relativeHeading < 20) || target.currentSpeed < 0.01f)
        {
            Seek(target.transform.position);
            return;
        }
        
        float lookAhead = targetDir.magnitude / (agent.speed + target.speed);
        Seek(target.transform.position + target.transform.forward * lookAhead);
    }

    // Run from a moving threat
    private void Evade()
    {
        Vector3 targetDir = target.transform.position - transform.position;
        
        float lookAhead = targetDir.magnitude / (agent.speed + target.currentSpeed);
        Flee(target.transform.position + target.transform.forward * lookAhead);
    }

    // Move aimlessly but smoothly
    private void Wander()
    {
        wanderTarget += new Vector3(Random.Range(-1f, 1f) * wanderJitter, 0, Random.Range(-1f, 1f) * wanderJitter);

        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = gameObject.transform.InverseTransformVector(targetLocal);

        Seek(targetWorld);
    }

    private void Hide()
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;

        for (int i = 0; i < World.Instance.GetHidingSpots().Length; i++)
        {
            Vector3 hiderDir = World.Instance.GetHidingSpots()[i].transform.position - target.transform.position;
            Vector3 hidePos = World.Instance.GetHidingSpots()[i].transform.position + hiderDir.normalized * 5;

            if (Vector3.Distance(transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                dist = Vector3.Distance(transform.position, hidePos);
            }
        }

        Seek(chosenSpot);
    }

    private void CleverHide()
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        GameObject chosenGO = World.Instance.GetHidingSpots()[0];

        for (int i = 0; i < World.Instance.GetHidingSpots().Length; i++)
        {
            Vector3 hiderDir = World.Instance.GetHidingSpots()[i].transform.position - target.transform.position;
            Vector3 hidePos = World.Instance.GetHidingSpots()[i].transform.position + hiderDir.normalized * 5;

            if (Vector3.Distance(transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                chosenDir = hiderDir;
                chosenGO = World.Instance.GetHidingSpots()[i];
                dist = Vector3.Distance(transform.position, hidePos);
            }
        }

        Collider hideCol = chosenGO.GetComponent<Collider>();
        Ray backRay = new Ray(chosenSpot, -chosenDir.normalized);
        RaycastHit info;
        float distance = 100f;

        hideCol.Raycast(backRay, out info, distance);

        Seek(info.point + chosenDir.normalized * 5);
    }

    private bool CanSeeTarget()
    {
        RaycastHit raycastInfo;
        Vector3 rayToTarget = target.transform.position - transform.position;

        float lookAngle = Vector3.Angle(transform.forward, rayToTarget);

        if (lookAngle < 60 && Physics.Raycast(transform.position, rayToTarget, out raycastInfo))
        {
            if (raycastInfo.transform.gameObject.tag == "cop") return true;
        }
        return false;
    }

    private bool CanSeeMe()
    {
        Vector3 rayToTarget = this.transform.position - target.transform.position;

        float lookAngle = Vector3.Angle(target.transform.forward, rayToTarget);

        if (lookAngle < 60) return true;
        return false;
    }

    private bool IsTargetInRange()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < 10) return true;
        return false;
    }

    private void BehaviourCooldown()
    {
        coolDown = false;
    }
}
