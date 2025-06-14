using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum STATE
    {
        Idle, Patrol, Pursue, Attack, Sleep
    }

    public enum EVENT
    {
        Enter, Update, Exit
    }

    public STATE name;
    protected EVENT stage;
    protected GameObject npc;
    protected NavMeshAgent agent;
    protected Animator anim;
    protected Transform player;
    protected AudioSource audioSource;
    protected State nextState;

    private float visDist = 10f;
    private float visAngle = 30f;
    private float shootDist = 7f;

    public State(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player, AudioSource _audioSource)
    {
        npc = _npc;
        agent = _agent;
        anim = _anim;
        player = _player;
        audioSource = _audioSource;
        stage = EVENT.Enter;
    }

    public virtual void Enter() { stage = EVENT.Update; }
    public virtual void Update() { stage = EVENT.Update; }
    public virtual void Exit() { stage = EVENT.Exit; }

    public State Process()
    {
        if (stage == EVENT.Enter) Enter();
        if (stage == EVENT.Update) Update();
        if (stage == EVENT.Exit)
        {
            Exit(); 
            return nextState;
        }
        return this;
    }

    public bool CanSeePlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);

        if (direction.magnitude < visDist && angle < visAngle)
        {
            return true;
        }
        return false;
    }

    public bool CanAttackPlayer()
    {
        Vector3 direction = player.position - npc.transform.position;

        if (direction.magnitude < shootDist)
        {
            return true;
        }
        return false;
    }
}
