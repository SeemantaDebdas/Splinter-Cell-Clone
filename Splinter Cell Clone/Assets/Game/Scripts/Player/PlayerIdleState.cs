using System;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player player, PlayerStatemachine statemachine) : base(player, statemachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.InputReader.OnMovePerformed += SwitchToMoveState;
        player.InputReader.OnCrouchUpdated += SwitchToCrouchIdleState;


        player.Animator.CrossFadeInFixedTime("Idle", 0.1f);
    }

    public override void Exit()
    {
        base.Exit();

        player.InputReader.OnMovePerformed -= SwitchToMoveState;
        player.InputReader.OnCrouchUpdated -= SwitchToCrouchIdleState;

    }

    private void SwitchToCrouchIdleState(bool shouldCrouch)
    {
        if (shouldCrouch)
            statemachine.SwitchState(player.CrouchIdleState);
    }


    void SwitchToMoveState(Vector2 vector) => statemachine.SwitchState(player.MoveState);

    public override void Update()
    {
        base.Update();
    }
}
