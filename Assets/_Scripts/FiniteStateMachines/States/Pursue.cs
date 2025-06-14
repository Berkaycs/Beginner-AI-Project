using UnityEngine;
using UnityEngine.AI;

public class Pursue : State
{
    public Pursue(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, AudioSource _audioSource) : base(_npc, _agent, _anim, _player, _audioSource)
    {
        name = STATE.Pursue;
        agent.speed = 5;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        Debug.Log("Enter pursue state");
        anim.SetTrigger("isRunning");
        base.Enter();
    }

    public override void Update()
    {
        agent.SetDestination(player.position);

        if (agent.hasPath)
        {
            if (CanAttackPlayer())
            {
                nextState = new Attack(npc, agent, anim, player, audioSource);
                stage = EVENT.Exit;
            }
            else if (!CanSeePlayer())
            {
                nextState = new Patrol(npc, agent, anim, player, audioSource);
                stage = EVENT.Exit;
            }
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isRunning");
        base.Exit();
    }
}
