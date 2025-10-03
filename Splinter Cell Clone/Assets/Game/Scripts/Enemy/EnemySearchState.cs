using UnityEngine;

public class EnemySearchState : EnemyState
{
    private Vector3 investigationCenter;
    private Vector3 currentSearchPoint;
    private float searchTimer;
    private float searchDuration = 15f; // total search time
    private float moveInterval = 5f;   // how often to pick a new search point
    private float searchRadius = 10f;   // radius around the center
    public EnemySearchState(Enemy enemy, EnemyStatemachine statemachine) : base(enemy, statemachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        investigationCenter = enemy.Awareness.LastKnownPlayerPosition; // or set externally
        PickNewSearchPoint();
        searchTimer = searchDuration;

        enemy.Animator.CrossFadeInFixedTime("AimLocomotion", 0.1f);
        enemy.Agent.speed = enemy.AimMoveSpeed;
    }

    public override void Exit()
    {
        base.Exit();

        enemy.ClearInvestigation();
    }

    public override void Update()
    {
        base.Update();

        // look for player OnAwarenessIncrease
        // if (enemy.Awareness.PlayerTransform != null)
        // {
        //     statemachine.ChangeState(enemy.ChaseState);
        //     return;
        // }

        // move around searching
        if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance < 0.5f)
        {
            if (stateTimer <= 0)
            {
                PickNewSearchPoint();
                stateTimer = moveInterval;
            }
        }

        // end search after time
        searchTimer -= Time.deltaTime;
        if (searchTimer <= 0)
        {
            statemachine.SwitchState(enemy.IdleState);
        }
    }

    private void PickNewSearchPoint()
    {
        if (NavMeshUtility.TryGetRandomPoint(investigationCenter, searchRadius, out var point))
        {
            currentSearchPoint = point;
            enemy.Agent.SetDestination(currentSearchPoint);
        }
    }
}