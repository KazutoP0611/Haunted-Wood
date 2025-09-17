using UnityEngine;

public class Player_DeadState : PlayerEntityState
{
    public Player_DeadState(StateMachine stateMachine, Player player, string stateName) : base(stateMachine, player, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(0, 0);
    }
}
