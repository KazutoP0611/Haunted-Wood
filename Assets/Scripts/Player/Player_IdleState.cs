using UnityEngine;

public class Player_IdleState : PlayerEntityState
{
    public Player_IdleState(StateMachine stateMachine, Player player, string stateName) : base(stateMachine, player, stateName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.moveInput.x != 0 || player.moveInput.y != 0)
            stateMachine.ChangeState(player.playerWalkState);
    }
}
