using UnityEngine;

public class EnemyMoveState : EnemyState
{
    public EnemyMoveState(Enemy enemy, EnemyStatemachine statemachine) : base(enemy, statemachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy.Animator.CrossFadeInFixedTime("Move", 0.1f);
    }

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