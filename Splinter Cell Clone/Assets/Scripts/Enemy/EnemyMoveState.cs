using UnityEngine;

public class EnemyMoveState : EnemyState
{
    public EnemyMoveState(Enemy enemy, EnemyStatemachine statemachine) : base(enemy, statemachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy.Awareness.OnAwarenessIncreaseStart += Awareness_OnAwarenessIncreaseStart;

        enemy.Agent.speed = 0.85f;
        enemy.Animator.CrossFadeInFixedTime("Move", 0.1f);
        enemy.Animator.SetFloat("moveSpeed", enemy.Agent.speed);
        enemy.Awareness.ResetAwarenessChangeRate();
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
        Vector3 currentWaypoint = enemy.GetCurrentWaypoint();

        if (Vector3.Distance(currentWaypoint, enemy.transform.position) < enemy.Agent.stoppingDistance)
        {
            enemy.IncrementCurrentWaypointIndex();
            statemachine.SwitchState(enemy.IdleState);
            return;
        }

        enemy.Agent.SetDestination(currentWaypoint);
    }
}