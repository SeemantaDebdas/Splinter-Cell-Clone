using UnityEngine;

public class EnemyInvestigationState : EnemyState
{
    private Vector3 targetPos;
    private InvestigationType type;

    bool hasReachedInvestigationPoint = false;

    public EnemyInvestigationState(Enemy enemy, EnemyStatemachine statemachine)
        : base(enemy, statemachine) { }

    public override void Enter()
    {
        base.Enter();

        if (enemy.CurrentInvestigation.HasValue)
        {
            var request = enemy.CurrentInvestigation.Value;
            targetPos = request.Position;
            type = request.Type;

            enemy.Agent.isStopped = false;
            enemy.Agent.SetDestination(targetPos);
        }
        else
        {
            // fallback if somehow entered with no request
            statemachine.SwitchState(enemy.IdleState);
        }

        hasReachedInvestigationPoint = false;
    }

    public override void Update()
    {
        base.Update();

        // If reached investigation spot
        if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance && !hasReachedInvestigationPoint)
        {
            hasReachedInvestigationPoint = true;
            // Look around / play animation
            enemy.FaceTarget(targetPos);

            stateTimer = 3f; // search for a few seconds

            enemy.Agent.ResetPath();
            enemy.Animator.CrossFadeInFixedTime("InvestigationIdle", 0.1f);
        }

        if (hasReachedInvestigationPoint && stateTimer < 0f)
        {
            statemachine.SwitchState(enemy.SearchState);
        }
    }


    public override void Exit()
    {
        base.Exit();
        enemy.ClearInvestigation();
    }
}
