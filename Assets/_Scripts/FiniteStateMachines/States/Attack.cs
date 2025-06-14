using UnityEngine;
using UnityEngine.AI;

public class Attack : State
{
    private float rotationSpeed = 2f;

    public Attack(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, AudioSource _audioSource) : base(_npc, _agent, _anim, _player, _audioSource)
    {
        name = STATE.Attack;
    }

    public override void Enter()
    {
        Debug.Log("Enter attack state");
        anim.SetTrigger("isShooting");
        agent.isStopped = true;
        audioSource.Play();
        base.Enter();
    }

    public override void Update()
    {
        // rotate npc while attacking
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        direction.y = 0;

        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation,
                                                  Quaternion.LookRotation(direction), 
                                                  rotationSpeed * Time.deltaTime);

        if (!CanAttackPlayer())
        {
            nextState = new Idle(npc, agent, anim, player, audioSource);
            stage = EVENT.Exit;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isShooting");
        audioSource.Stop();
        base.Exit();
    }
}
