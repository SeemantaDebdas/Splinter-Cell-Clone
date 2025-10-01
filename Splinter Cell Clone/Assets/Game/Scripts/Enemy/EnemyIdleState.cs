using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(Enemy enemy, EnemyStatemachine statemachine) : base(enemy, statemachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.IdleTime;

        enemy.Animator.CrossFadeInFixedTime("Idle", 0.1f);

    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            statemachine.SwitchState(enemy.MoveState);
    }
}