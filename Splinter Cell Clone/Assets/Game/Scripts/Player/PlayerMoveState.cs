using System;
using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(Player player, PlayerStatemachine statemachine) : base(player, statemachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.InputReader.OnMoveCancelled += SwitchToIdleState;
    }
    public override void Exit()
    {
        base.Exit();

        player.InputReader.OnMoveCancelled -= SwitchToIdleState;

    }

    private void SwitchToIdleState() => statemachine.SwitchState(player.IdleState);

    public override void Update()
    {
        base.Update();

        Vector2 moveInput = player.InputReader.MoveInput;

        Vector3 moveDir = new(moveInput.x, 0, moveInput.y);
        moveDir = Camera.main.transform.TransformDirection(moveDir);
        moveDir.y = 0;
        moveDir.Normalize();

        player.Controller.Move(player.MoveSpeed * Time.deltaTime * moveDir);

        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, targetRotation, player.RotationSpeed * Time.deltaTime);
    }

}
