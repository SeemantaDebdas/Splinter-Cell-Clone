using UnityEngine;

public class EnemyState
{
    protected Enemy enemy;
    protected EnemyStatemachine statemachine;
    protected string animation;

    protected float stateTimer;

    public EnemyState(Enemy enemy, EnemyStatemachine statemachine)
    {
        this.enemy = enemy;
        this.statemachine = statemachine;
    }

    public virtual void Enter()
    {
        Debug.Log("Entered" + this);
    }

    public virtual void Exit()
    {

    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }
}
