using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    [field: SerializeField] public float IdleTime { get; private set; }
    [SerializeField] List<Transform> waypointList;
    int currentWaypointIndex = 0;

    public Animator Animator { get; private set; }
    public NavMeshAgent Agent { get; private set; }

    protected EnemyStatemachine statemachine;
    public EnemyIdleState IdleState { get; private set; }
    public EnemyMoveState MoveState { get; private set; }
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
    }

    private void InitializeStates()
    {
        statemachine = new EnemyStatemachine();

        IdleState = new EnemyIdleState(this, statemachine);
        MoveState = new EnemyMoveState(this, statemachine);

        statemachine.Initialize(IdleState);
    }

    public Vector3 GetCurrentWaypoint()
    {
        return waypointList[currentWaypointIndex].position;
    }

    public void IncrementCurrentWaypointIndex()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypointList.Count;
    }

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
