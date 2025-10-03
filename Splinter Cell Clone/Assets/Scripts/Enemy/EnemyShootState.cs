using UnityEngine;

public class EnemyShootState : EnemyState
{
    float minDistanceFromPlayer = 3f;
    float lastTimeShot = -Mathf.Infinity;
    float bulletsPerSecond = 5;
    public EnemyShootState(Enemy enemy, EnemyStatemachine statemachine) : base(enemy, statemachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy.Awareness.OnAwarenessDecreaseStart += Awareness_OnAwarenessDecreaseStart;
        enemy.OnInvestigationUpdated += Enemy_OnInvestigationUpdated;

        enemy.Animator.SetLayerWeight(1, 1);
        enemy.Animator.CrossFadeInFixedTime("AimLocomotion", 0.1f);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.Awareness.OnAwarenessDecreaseStart -= Awareness_OnAwarenessDecreaseStart;
        enemy.OnInvestigationUpdated -= Enemy_OnInvestigationUpdated;
        enemy.Animator.SetLayerWeight(1, 0);
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

        if (enemy.Awareness.PlayerTarget == null)
            return;

        Vector3 playerPosition = enemy.Awareness.PlayerTarget.position;
        enemy.FaceTarget(playerPosition);

        float distanceToPlayer = Vector3.Distance(playerPosition, enemy.transform.position);

        if (distanceToPlayer > enemy.ShootRadius + 0.5f)
        {
            statemachine.SwitchState(enemy.ChaseState);
            return;
        }

        if (distanceToPlayer <= minDistanceFromPlayer)
        {
            enemy.Agent.speed = 0f;
        }
        else
        {
            enemy.Agent.speed = 0.7f;
        }

        Shoot();

        enemy.Animator.SetFloat("moveSpeed", enemy.Agent.speed, 0.1f, Time.deltaTime);
    }

    void Shoot()
    {
        if (Time.time > lastTimeShot + 1 / bulletsPerSecond)
        {
            Debug.Log("Shooting");
            enemy.Animator.CrossFadeInFixedTime("Shoot", 0.1f, 1, 0);
            lastTimeShot = Time.time;
        }
    }
}