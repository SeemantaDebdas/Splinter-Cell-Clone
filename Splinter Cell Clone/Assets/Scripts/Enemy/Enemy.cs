using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Awareness))]
public class Enemy : MonoBehaviour
{
    [field: Header("Movement")]
    [field: SerializeField] public float IdleTime { get; private set; }
    [SerializeField] List<Transform> waypointList;
    int currentWaypointIndex = 0;

    [field: Header("Aggression")]
    [field: SerializeField] public float ShootRadius { get; private set; } = 5f;
    [field: SerializeField] public float AimMoveSpeed { get; private set; } = 0.7f;

    public Animator Animator { get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public Awareness Awareness { get; private set; }

    public InvestigationRequest? CurrentInvestigation { get; private set; }
    public event Action OnInvestigationUpdated;

    protected EnemyStatemachine statemachine;
    public EnemyIdleState IdleState { get; private set; }
    public EnemyMoveState MoveState { get; private set; }
    public EnemySuspiscionIdleState SuspiscionIdleState { get; private set; }
    public EnemyChaseState ChaseState { get; private set; }
    public EnemyShootState ShootState { get; private set; }
    public EnemyInvestigationState InvestigationState { get; private set; }
    public EnemySearchState SearchState { get; private set; }
    private void Awake()
    {
        InitializeComponents();
        InitializeStates();
    }

    void Update()
    {
        statemachine.Update();
    }

    void InitializeComponents()
    {
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        Awareness = GetComponent<Awareness>();
    }

    private void InitializeStates()
    {
        statemachine = new EnemyStatemachine();

        IdleState = new EnemyIdleState(this, statemachine);
        MoveState = new EnemyMoveState(this, statemachine);
        SuspiscionIdleState = new EnemySuspiscionIdleState(this, statemachine);
        ChaseState = new EnemyChaseState(this, statemachine);
        ShootState = new EnemyShootState(this, statemachine);
        InvestigationState = new EnemyInvestigationState(this, statemachine);
        SearchState = new EnemySearchState(this, statemachine);

        statemachine.Initialize(IdleState);
    }

    public EnemyState GetCurrentState() => statemachine.CurrentState;

    public Vector3 GetCurrentWaypoint()
    {
        return waypointList[currentWaypointIndex].position;
    }

    public void IncrementCurrentWaypointIndex()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypointList.Count;
    }

    public void FaceTarget(Vector3 position, float rotateSpeed = 270f)
    {
        Vector3 lookDir = (position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    #region  Investigation
    public void SetInvestigation(Vector3 position, InvestigationType type)
    {
        CurrentInvestigation = new InvestigationRequest(position, type);
        // statemachine.SwitchState(InvestigationState);
        OnInvestigationUpdated?.Invoke();
    }

    public void ClearInvestigation() => CurrentInvestigation = null;

    #endregion

    void OnDrawGizmosSelected()
    {
        if (waypointList.Count == 0)
            return;

        for (int i = 0; i < waypointList.Count - 1; i++)
        {
            Gizmos.DrawLine(waypointList[i].position, waypointList[i + 1].position);
        }
        Gizmos.DrawLine(waypointList[^1].position, waypointList[0].position);
    }
}
