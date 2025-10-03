using System;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(Enemy enemy, EnemyStatemachine statemachine) : base(enemy, statemachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy.Awareness.OnAwarenessIncreaseStart += Awareness_OnAwarenessIncreaseStart;

        stateTimer = enemy.IdleTime;
        enemy.Animator.CrossFadeInFixedTime("Idle", 0.1f);

    }

    public override void Exit()
    {
        base.Exit();

        enemy.Awareness.OnAwarenessIncreaseStart -= Awareness_OnAwarenessIncreaseStart;
    }

    void Awareness_OnAwarenessIncreaseStart() => statemachine.SwitchState(enemy.SuspiscionIdleState);

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            statemachine.SwitchState(enemy.MoveState);
    }
}