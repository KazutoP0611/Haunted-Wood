using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Enemy_WalkState : EnemyEntityState
{
    public Enemy_WalkState(StateMachine stateMachine, Enemy enemy, string stateName) : base(stateMachine, enemy, stateName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (enemy.alive)
        {
            if (player.alive/* && enemy.isActivating*/)
            {
                Vector3 normalizedVector = vectorToPlayer.normalized;
                enemy.SetVelocity(normalizedVector.x, normalizedVector.y);
                //enemy.transform.position += vectorToPlayer * Time.deltaTime * enemy.f_Speed;

                if (vectorToPlayer.magnitude <= distanceFromPlayertoAttack)
                    stateMachine.ChangeState(enemy.enemyAttackState);
            }
            else/* if (!enemy.isActivating)*/
                stateMachine.ChangeState(enemy.enemyIdleState);

        }
    }
}
