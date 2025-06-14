using UnityEngine;
using UnityEngine.AI;

public class Patrol : State
{
    private int currentIndex = -1;
    
    public Patrol(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, AudioSource _audioSource) : base(_npc, _agent, _anim, _player, _audioSource)
    {
        name = STATE.Patrol;
        agent.speed = 2;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        Debug.Log("Enter patrol state");
        float lastDistance = Mathf.Infinity;

        for (int i = 0; i < GameEnvironment.Singleton.Checkpoints.Count; i++)
        {
            GameObject thisWP = GameEnvironment.Singleton.Checkpoints[i];
            float distance = Vector3.Distance(npc.transform.position, thisWP.transform.position);
            if (distance < lastDistance)
            {
                lastDistance = distance;
                currentIndex = i - 1; // because we increase it in the update before set destination
            }
        }

        anim.SetTrigger("isWalking");
        base.Enter();   
    }

    public override void Update()
    {
        if (agent.remainingDistance < 1)
        {
            if (currentIndex >= GameEnvironment.Singleton.Checkpoints.Count - 1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }

            agent.SetDestination(GameEnvironment.Singleton.Checkpoints[currentIndex].transform.position);
        }

        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, anim, player, audioSource);
            stage = EVENT.Exit;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isWalking");
        base.Exit();
    }
}
