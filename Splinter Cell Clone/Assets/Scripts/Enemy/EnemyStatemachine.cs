using UnityEngine;

public class EnemyStatemachine
{
    public EnemyState CurrentState { get; private set; }

    public void Initialize(EnemyState initialState)
    {
        CurrentState = initialState;
        CurrentState?.Enter();
    }

    public void SwitchState(EnemyState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }

    public void Update()
    {
        CurrentState?.Update();
    }
}
