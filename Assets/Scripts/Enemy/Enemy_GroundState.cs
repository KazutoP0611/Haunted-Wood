using UnityEngine;

public class Enemy_GroundState : EnemyEntityState
{
    public Enemy_GroundState(StateMachine stateMachine, Enemy enemy, string stateName) : base(stateMachine, enemy, stateName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (enemy.PlayerDetected() == true)
            stateMachine.ChangeState(enemy.enemyBattleState);
    }
}
