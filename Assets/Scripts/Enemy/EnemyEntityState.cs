using UnityEngine;

public class EnemyEntityState : EntityState
{
    protected Enemy enemy;
    protected Player player;
    protected float distanceFromPlayertoAttack;
    protected Vector3 vectorToPlayer = Vector3.zero;

    public EnemyEntityState(StateMachine stateMachine, Enemy enemy, string stateName) : base(stateMachine, stateName)
    {
        this.enemy = enemy;
        anim = enemy.anim;
            
        player = enemy.player;
        distanceFromPlayertoAttack = enemy.f_DistanceFromPlayerToAttack;
    }

    public override void Update()
    {
        base.Update();

        if (player != null)
        {
            if (player.alive/* && enemy.isActivating*/)
                vectorToPlayer = player.transform.position - enemy.transform.position;
            else if (!player.alive && !enemy.attacking)
                stateMachine.ChangeState(enemy.enemyIdleState);
        }
    }
}
