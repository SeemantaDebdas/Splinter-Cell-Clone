using System;
using UnityEngine;

public class PlayerCrouchIdleState : PlayerState
{
    public PlayerCrouchIdleState(Player player, PlayerStatemachine statemachine) : base(player, statemachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.InputReader.OnMovePerformed += SwitchToMoveState;
        player.InputReader.OnCrouchUpdated += SwitchToIdleState;

        player.Animator.CrossFadeInFixedTime("CrouchIdle", 0.1f);
    }

    public override void Exit()
    {
        base.Exit();

        player.InputReader.OnMovePerformed -= SwitchToMoveState;
        player.InputReader.OnCrouchUpdated -= SwitchToIdleState;
    }

    private void SwitchToIdleState(bool shouldCrouch)
    {
        if (!shouldCrouch)
            statemachine.SwitchState(player.IdleState);
    }


    void SwitchToMoveState(Vector2 vector) => statemachine.SwitchState(player.CrouchMoveState);

    public override void Update()
    {
        base.Update();
    }
}
