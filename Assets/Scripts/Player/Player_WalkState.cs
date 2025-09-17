using UnityEngine;

public class Player_WalkState : PlayerEntityState
{
    public Player_WalkState(StateMachine stateMachine, Player player, string stateName) : base(stateMachine, player, stateName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.moveInput == Vector2.zero)
            stateMachine.ChangeState(player.playerIdleState);
    }
}
