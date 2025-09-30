using UnityEngine;

public class PlayerStatemachine
{
    public PlayerState CurrentState { get; private set; }

    public void Initialize(PlayerState initialState)
    {
        CurrentState = initialState;
        CurrentState?.Enter();
    }

    public void SwitchState(PlayerState newState)
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
