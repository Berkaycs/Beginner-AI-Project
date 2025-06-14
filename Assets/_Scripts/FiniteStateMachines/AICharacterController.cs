using UnityEngine;
using UnityEngine.AI;
public class AICharacterController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator anim;
    public Transform player;
    public AudioSource audioSource;
    private State currentState;

    private void Start()
    {
        currentState = new Idle(gameObject, agent, anim, player, audioSource);
    }

    private void Update()
    {
        currentState = currentState.Process();
    }
}
