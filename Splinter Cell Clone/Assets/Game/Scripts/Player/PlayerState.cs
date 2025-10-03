using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStatemachine statemachine;
    protected string animation;

    protected float stateTimer;

    public PlayerState(Player player, PlayerStatemachine statemachine)
    {
        this.player = player;
        this.statemachine = statemachine;
    }

    public virtual void Enter()
    {
    }

    public virtual void Exit()
    {

    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }
}
