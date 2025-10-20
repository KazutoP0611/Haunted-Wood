using UnityEngine;

public class Enemy_BattleState : EnemyEntityState
{
    public Enemy_BattleState(StateMachine stateMachine, Enemy enemy, string stateName) : base(stateMachine, enemy, stateName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (ShouldBackOff())
            enemy.BackOff();

        if (WithinRangeToAttack() && enemy.PlayerDetected())
            stateMachine.ChangeState(enemy.enemyAttackState);
        else
            stateMachine.ChangeState(enemy.enemyWalkState);
    }

    private bool WithinRangeToAttack() => enemy.DistanceToPlayer() <= enemy.distanceFromPlayertoAttack;

    private bool ShouldBackOff() => vectorToPlayer.magnitude <= enemy.minRangeToPlayertoAttack;
}
