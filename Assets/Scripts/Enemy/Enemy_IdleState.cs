using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Enemy_IdleState : EnemyEntityState
{
    public Enemy_IdleState(StateMachine stateMachine, Enemy enemy, string stateName) : base(stateMachine, enemy, stateName)
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

        if (player.alive/* && enemy.isActivating*/)
        {
            //Debug.Log("Idle");
            if (vectorToPlayer.magnitude > distanceFromPlayertoAttack)
                stateMachine.ChangeState(enemy.enemyWalkState);
            else
                stateMachine.ChangeState(enemy.enemyAttackState);
        }
    }
}
