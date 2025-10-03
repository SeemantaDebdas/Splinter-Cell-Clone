using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(FieldOfView))]
public class Awareness : MonoBehaviour
{
    // --- Dependencies ---
    private FieldOfView fieldOfView;

    // --- Configurable Values ---
    [SerializeField] private float awarenessLevel = 0f;
    [SerializeField] private float awarenessChangeRate = 0.25f;

    // --- State ---
    public Transform PlayerTarget { get; private set; } = null;   // Null = not in sight
    private Coroutine awarenessRoutine;

    // --- Events ---
    public event Action OnAwarenessIncreaseStart;   // Player entered FOV, meter starts filling
    public event Action OnAwarenessMaxed;           // Awareness reached full (alert)
    public event Action OnAwarenessDecreaseStart;   // Player left FOV, meter starts decreasing
    public event Action OnAwarenessEmptied;         // Awareness dropped back to 0

    // --- Unity Methods ---
    private void Awake()
    {
        fieldOfView = GetComponent<FieldOfView>();
    }

    private void OnEnable()
    {
        fieldOfView.OnVisibleObjectsUpdated += HandleVisibleObjectsUpdated;
    }

    private void OnDisable()
    {
        fieldOfView.OnVisibleObjectsUpdated -= HandleVisibleObjectsUpdated;
    }

    // --- Handlers ---
    private void HandleVisibleObjectsUpdated()
    {
        if (awarenessRoutine != null)
        {
            StopCoroutine(awarenessRoutine);
            awarenessRoutine = null;
        }

        Transform foundPlayer = null;

        for (int i = 0; i < fieldOfView.VisibleObjects.Count; i++)
        {
            if (fieldOfView.VisibleObjects[i].CompareTag("Player"))
            {
                foundPlayer = fieldOfView.VisibleObjects[i].transform;
                break;
            }
        }

        // Player just became visible
        if (foundPlayer != null && PlayerTarget == null)
        {
            PlayerTarget = foundPlayer;
            OnAwarenessIncreaseStart?.Invoke();
            awarenessRoutine = StartCoroutine(UpdateAwarenessLevel(increasing: true));
        }
        // Player just became invisible
        else if (foundPlayer == null && PlayerTarget != null)
        {
            PlayerTarget = null;
            OnAwarenessDecreaseStart?.Invoke();
            awarenessRoutine = StartCoroutine(UpdateAwarenessLevel(increasing: false));
        }
    }

    // --- Coroutines ---
    private IEnumerator UpdateAwarenessLevel(bool increasing)
    {
        float rate = increasing ? awarenessChangeRate : -awarenessChangeRate;

        while (true)
        {
            awarenessLevel = Mathf.Clamp01(awarenessLevel + Time.deltaTime * rate);

            if (increasing && awarenessLevel >= 1f)
            {
                OnAwarenessMaxed?.Invoke();
                yield break;
            }

            if (!increasing && awarenessLevel <= 0f)
            {
                OnAwarenessEmptied?.Invoke();
                yield break;
            }

            yield return null;
        }
    }

    // --- Public API ---
    public bool HasLineOfSightToPlayer => PlayerTarget != null;
}
