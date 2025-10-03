using System;
using UnityEngine;

public class EnemySuspiscionIdleState : EnemyState
{
    public EnemySuspiscionIdleState(Enemy enemy, EnemyStatemachine statemachine) : base(enemy, statemachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy.Agent.ResetPath();
        enemy.Animator.CrossFadeInFixedTime("AimIdle", 0.1f);
        enemy.Awareness.OnAwarenessEmptied += Awareness_OnAwarenessEmptied;
        enemy.Awareness.OnAwarenessMaxed += Awareness_OnAwarenessMaxed;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.Awareness.OnAwarenessEmptied -= Awareness_OnAwarenessEmptied;
        enemy.Awareness.OnAwarenessMaxed -= Awareness_OnAwarenessMaxed;
    }

    public override void Update()
    {
        base.Update();

        Transform playerTarget = enemy.Awareness.PlayerTarget;
        if (playerTarget != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation((playerTarget.position - enemy.transform.position).normalized);
            enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, targetRotation, 360f * Time.deltaTime);
        }
    }

    void Awareness_OnAwarenessEmptied() => statemachine.SwitchState(enemy.IdleState);
    void Awareness_OnAwarenessMaxed() => statemachine.SwitchState(enemy.ChaseState);

}