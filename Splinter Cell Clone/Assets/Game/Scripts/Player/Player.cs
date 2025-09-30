using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    [field: Header("Stats")]
    [field: SerializeField] public float WalkSpeed { get; private set; } = 1.7f;
    [field: SerializeField] public float RunSpeed { get; private set; } = 2.75f;
    [field: SerializeField] public float RotationSpeed { get; private set; } = 60;

    public float MoveSpeed { get; private set; } = 0;

    [field: Header("References")]
    [field: SerializeField] public InputReader InputReader { get; private set; }
    public CharacterController Controller { get; private set; }
    public Animator Animator { get; private set; }
    PlayerStatemachine statemachine;

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }

    void Awake()
    {
        InitializeComponents();
        InitializeStats();
        InitializeStates();
    }

    void InitializeStats()
    {
        HandleMoveSpeed(false);
    }

    void InitializeComponents()
    {
        Controller = GetComponent<CharacterController>();
        Animator = GetComponent<Animator>();
    }

    private void InitializeStates()
    {
        statemachine = new PlayerStatemachine();

        IdleState = new PlayerIdleState(this, statemachine);
        MoveState = new PlayerMoveState(this, statemachine);

        statemachine.Initialize(IdleState);
    }


    void OnEnable()
    {
        InputReader.OnSprintUpdated += HandleMoveSpeed;
    }

    void OnDisable()
    {
        InputReader.OnSprintUpdated -= HandleMoveSpeed;

    }

    private void HandleMoveSpeed(bool shouldSprint)
    {
        MoveSpeed = shouldSprint ? RunSpeed : WalkSpeed;
    }

    void Update()
    {
        statemachine.Update();
    }
}
