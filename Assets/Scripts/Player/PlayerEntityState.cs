using UnityEngine;

public class PlayerEntityState : EntityState
{
    protected Player player;

    public PlayerEntityState(StateMachine stateMachine, Player player, string stateName) : base(stateMachine, stateName)
    {
        this.player = player;

        anim = player.anim;
    }
}
