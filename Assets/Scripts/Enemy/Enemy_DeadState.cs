using UnityEngine;

public class Enemy_DeadState : EnemyEntityState
{
    public Enemy_DeadState(StateMachine stateMachine, Enemy enemy, string stateName) : base(stateMachine, enemy, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetVelocity(0, 0);
    }
}
