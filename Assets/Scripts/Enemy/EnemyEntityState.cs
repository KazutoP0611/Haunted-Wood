using UnityEngine;

public class EnemyEntityState : EntityState
{
    protected Enemy enemy;
    protected Player player;

    protected Vector3 vectorToPlayer = Vector3.zero;
    protected Vector3 targetPosition = Vector3.zero;

    public EnemyEntityState(StateMachine stateMachine, Enemy enemy, string stateName) : base(stateMachine, stateName)
    {
        this.enemy = enemy;
        anim = enemy.anim;
        rb = enemy.rb;
            
        player = enemy.player;
    }

    public override void Update()
    {
        base.Update();

        if (player != null)
        {
            if (player.alive)
            {
                vectorToPlayer = player.transform.position - enemy.transform.position;
                targetPosition = new Vector2(player.transform.position.x + (enemy.attackOffset.x * -enemy.DirectionToPlayer()), player.transform.position.y + enemy.attackOffset.y);
            }
            else if (!player.alive && !enemy.attacking)
                stateMachine.ChangeState(enemy.enemyIdleState);
        }
    }
}
