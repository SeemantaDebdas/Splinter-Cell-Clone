using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New InputReader", menuName = "Debdas/Input Reader")]
public class InputReader : ScriptableObject, Controls.IPlayerActions
{
    Controls controls;

    public Action OnMoveCancelled;
    public Action<Vector2> OnMovePerformed;
    public Vector2 MoveInput;
    private void OnEnable()
    {
        controls ??= new Controls();
        controls.Player.SetCallbacks(this);
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.canceled)
            OnMoveCancelled?.Invoke();

        if (context.performed)
            OnMovePerformed?.Invoke(context.ReadValue<Vector2>());

        MoveInput = context.ReadValue<Vector2>();
    }
}
