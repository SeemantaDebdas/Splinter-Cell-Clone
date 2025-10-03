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
        enemy.OnInvestigationUpdated += Enemy_OnInvestigationUpdated;

        enemy.Animator.CrossFadeInFixedTime("Move", 0.1f);
        enemy.Agent.speed = 2.5f;
    }

    public override void Exit()
    {
        base.Exit();

        enemy.Awareness.OnAwarenessDecreaseStart -= Awareness_OnAwarenessDecreaseStart;
        enemy.OnInvestigationUpdated -= Enemy_OnInvestigationUpdated;
    }

    void Enemy_OnInvestigationUpdated() => statemachine.SwitchState(enemy.InvestigationState);
    void Awareness_OnAwarenessDecreaseStart()
    {
        enemy.SetInvestigation(enemy.Awareness.LastKnownPlayerPosition, InvestigationType.LostPlayer);
        GhostProjectionHandler.Instance.InstantiateProjection();
    }

    public override void Update()
    {
        base.Update();


        float distanceToPlayer = Vector3.Distance(enemy.Awareness.PlayerTarget.position, enemy.transform.position);
        if (distanceToPlayer <= enemy.ShootRadius)
        {
            statemachine.SwitchState(enemy.ShootState);
            return;
        }

        enemy.Agent.SetDestination(enemy.Awareness.PlayerTarget.position);


        enemy.Animator.SetFloat("moveSpeed", enemy.Agent.speed, 0.1f, Time.deltaTime);

    }
}