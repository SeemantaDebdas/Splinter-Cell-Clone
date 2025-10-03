using System;
using UnityEngine;

public class PlayerMoveState : PlayerState
{
    float moveSpeed;
    public PlayerMoveState(Player player, PlayerStatemachine statemachine) : base(player, statemachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.InputReader.OnMoveCancelled += SwitchToIdleState;
        player.InputReader.OnCrouchUpdated += SwitchToCrouchIdleState;
        player.InputReader.OnSprintUpdated += HandleMoveSpeed;

        moveSpeed = player.WalkSpeed;
        player.Animator.CrossFadeInFixedTime("Move", 0.1f);
    }
    public override void Exit()
    {
        base.Exit();

        player.InputReader.OnMoveCancelled -= SwitchToIdleState;
        player.InputReader.OnCrouchUpdated -= SwitchToCrouchIdleState;
        player.InputReader.OnSprintUpdated -= HandleMoveSpeed;

    }

    private void HandleMoveSpeed(bool isSprinting)
    {
        moveSpeed = isSprinting ? player.RunSpeed : player.WalkSpeed;
    }

    private void SwitchToCrouchIdleState(bool shouldCrouch)
    {
        if (shouldCrouch)
            statemachine.SwitchState(player.CrouchMoveState);
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

        player.Controller.Move(moveSpeed * Time.deltaTime * moveDir);

        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, targetRotation, player.RotationSpeed * Time.deltaTime);

        player.Animator.SetFloat("moveSpeed", moveSpeed, 0.1f, Time.deltaTime);
    }

}
