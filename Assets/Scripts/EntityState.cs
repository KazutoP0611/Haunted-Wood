using UnityEngine;
using UnityEngine.PlayerLoop;

public abstract class EntityState
{
    protected StateMachine stateMachine;

    protected string stateName;
    protected Animator anim;
    protected Rigidbody2D rb;

    public EntityState(StateMachine stateMachine, string stateName)
    {
        this.stateMachine = stateMachine;
        this.stateName = stateName;
    }

    public virtual void Enter()
    {
        anim.SetBool(stateName, true);
    }

    public virtual void Update()
    {
        
    }

    public virtual void Exit()
    {
        anim.SetBool(stateName, false);
    }
}
