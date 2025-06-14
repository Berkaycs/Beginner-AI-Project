using UnityEngine;
using UnityEngine.AI;

public class ChomperAIController : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject player;
    public Animator animator;

    private void Update()
    {
        agent.SetDestination(player.transform.position);

        if (agent.remainingDistance < 2)
        {
            animator.SetBool("isMoving", false);
        }
        else
        {
            animator.SetBool("isMoving", true);
        }
    }
}
