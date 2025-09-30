using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [field: Header("Stats")]
    [field: SerializeField] public float MoveSpeed { get; private set; } = 5;
    [field: SerializeField] public float RotationSpeed { get; private set; } = 60;
    [field: Header("References")]
    [field: SerializeField] public InputReader InputReader { get; private set; }
    public CharacterController Controller { get; private set; }
    PlayerStatemachine statemachine;

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }

    void Awake()
    {
        Controller = GetComponent<CharacterController>();

        statemachine = new PlayerStatemachine();

        IdleState = new PlayerIdleState(this, statemachine);
        MoveState = new PlayerMoveState(this, statemachine);

        statemachine.Initialize(IdleState);
    }

    void Update()
    {
        statemachine.Update();
    }
}
