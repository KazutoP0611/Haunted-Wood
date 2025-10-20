using UnityEngine;

public class Enemy_AttackState : EnemyEntityState
{
    public Enemy_AttackState(StateMachine stateMachine, Enemy enemy, string stateName) : base(stateMachine, enemy, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetVelocity(0, 0);
    }

    public override void Update()
    {
        base.Update();

        if (!enemy.attacking)
            stateMachine.ChangeState(enemy.enemyBattleState);
    }
}
