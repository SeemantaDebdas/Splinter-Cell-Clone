using System;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(Enemy enemy, EnemyStatemachine statemachine) : base(enemy, statemachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy.Awareness.OnAwarenessDecreaseStart += Awareness_OnAwarenessDecreaseStart;

        enemy.Animator.CrossFadeInFixedTime("AimMove", 0.1f);
    }

    public override void Update()
    {
        base.Update();

        enemy.Agent.SetDestination(enemy.Awareness.PlayerTarget.position);

        float distanceToPlayer = Vector3.Distance(enemy.Awareness.PlayerTarget.position, enemy.transform.position);
        if (distanceToPlayer <= enemy.ShootRadius)
        {
            enemy.Agent.speed = 0.8f;
        }
        else
        {
            enemy.Agent.speed = 2.5f;
        }

        enemy.Animator.SetFloat("moveSpeed", enemy.Agent.speed, 0.1f, Time.deltaTime);

    }

    public override void Exit()
    {
        base.Exit();

        enemy.Awareness.OnAwarenessDecreaseStart -= Awareness_OnAwarenessDecreaseStart;
    }

    void Awareness_OnAwarenessDecreaseStart()
    {
        statemachine.SwitchState(enemy.SuspiscionIdleState);
    }
}