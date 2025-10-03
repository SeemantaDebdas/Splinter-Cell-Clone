using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    [field: Header("Stats")]
    [field: SerializeField] public float WalkSpeed { get; private set; } = 1.7f;
    [field: SerializeField] public float RunSpeed { get; private set; } = 2.75f;
    [field: SerializeField] public float CrouchWalkSpeed { get; private set; } = 1f;
    [field: SerializeField] public float CrouchRunSpeed { get; private set; } = 1.8f;
    [field: SerializeField] public float RotationSpeed { get; private set; } = 60;

    public float MoveSpeed { get; private set; } = 0;

    [field: Header("References")]
    [field: SerializeField] public InputReader InputReader { get; private set; }
    public CharacterController Controller { get; private set; }
    public Animator Animator { get; private set; }
    PlayerStatemachine statemachine;

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerCrouchIdleState CrouchIdleState { get; private set; }
    public PlayerCrouchMoveState CrouchMoveState { get; private set; }

    void Awake()
    {
        InitializeComponents();
        InitializeStates();
        InitializeStats();
    }

    void InitializeStats()
    {
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
        CrouchIdleState = new PlayerCrouchIdleState(this, statemachine);
        CrouchMoveState = new PlayerCrouchMoveState(this, statemachine);

        statemachine.Initialize(IdleState);
    }


    void Update()
    {
        statemachine.Update();
    }
}
