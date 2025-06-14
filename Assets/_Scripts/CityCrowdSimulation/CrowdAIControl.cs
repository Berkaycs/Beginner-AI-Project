using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrowdAIControl : MonoBehaviour {

    private GameObject[] goalLocations;
    private NavMeshAgent agent;
    private Animator anim;
    private float speedMultiplier;
    private float detectionRadius = 20;
    private float fleeRadius = 10;

    void Start() {

        agent = GetComponent<NavMeshAgent>();
        goalLocations = GameObject.FindGameObjectsWithTag("goal");
        int i = Random.Range(0, goalLocations.Length);
        agent.SetDestination(goalLocations[i].transform.position);
        anim = this.GetComponent<Animator>();

        // we tick the cycle offset's parameter option in walk animation inspector
        // with this, the walk animation can start different times for people
        anim.SetFloat("walkOffset", Random.Range(0f, 1f));

        ResetAgent();
    }

    void Update() {

        if (agent.remainingDistance < 1.0f) 
        {
            ResetAgent();
            int i = Random.Range(0, goalLocations.Length);
            agent.SetDestination(goalLocations[i].transform.position);
        }
    }

    private void ResetAgent()
    {
        anim.SetTrigger("isWalking");

        // set different speeds to walk animation
        speedMultiplier = Random.Range(0.1f, 1.3f);
        anim.SetFloat("speedMultiplier", speedMultiplier);
        agent.speed *= speedMultiplier;
        agent.angularSpeed = 120;
        agent.ResetPath();
    }

    public void DetectNewObstacle(Vector3 position)
    {
        if (Vector3.Distance(position, transform.position) < detectionRadius)
        {
            Vector3 fleeDir = (transform.position - position).normalized;
            Vector3 newGoal = transform.position + fleeDir * fleeRadius;

            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(newGoal, path);

            if (path.status != NavMeshPathStatus.PathInvalid)
            {
                agent.SetDestination(path.corners[path.corners.Length - 1]);
                anim.SetTrigger("isRunning");
                agent.speed = 10;
                agent.angularSpeed = 500;
            }
        }
    }
}