using UnityEngine;
using UnityEngine.AI;

public class Idle : State
{
    public Idle(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, AudioSource _audioSource) : base(_npc, _agent, _anim, _player, _audioSource)
    {
        name = STATE.Idle;
    }

    public override void Enter()
    {
        Debug.Log("Enter idle state");
        anim.SetTrigger("isIdle");
        base.Enter();
    }

    public override void Update()
    {
        //base.Update(); we dont need it already works in enter func this will work until exit

        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, anim, player, audioSource);
            stage = EVENT.Exit;
        }
        else if (Random.Range(0, 100) < 10)
        {
            nextState = new Patrol(npc, agent, anim, player, audioSource);
            stage = EVENT.Exit;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isIdle");
        base.Exit();
    }
}
