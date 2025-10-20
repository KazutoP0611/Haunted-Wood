using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Enemy_WalkState : Enemy_GroundState
{
    public Enemy_WalkState(StateMachine stateMachine, Enemy enemy, string stateName) : base(stateMachine, enemy, stateName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (enemy.alive)
        {
            if (player.alive)
            {
                Vector3 normalizedVector = (targetPosition - enemy.transform.position).normalized;
                enemy.SetVelocity(normalizedVector.x, normalizedVector.y);
            }
            else
                stateMachine.ChangeState(enemy.enemyIdleState);
        }
    }
}
