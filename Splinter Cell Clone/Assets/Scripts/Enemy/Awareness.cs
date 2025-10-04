using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(FieldOfView))]
public class Awareness : MonoBehaviour
{
    // --- Dependencies ---
    private FieldOfView fieldOfView;

    // --- Configurable Values ---
    [field: SerializeField] public float AwarenessLevel { get; private set; } = 0f;
    [SerializeField] private float awarenessChangeRate = 0.25f;
    float currentAwarenessChangeRate = 0.25f;

    // --- State ---
    public Transform PlayerTarget { get; private set; } = null;   // Null = not in sight
    public Vector3 LastKnownPlayerPosition { get; private set; } = default;
    private Coroutine awarenessRoutine;

    // NEW: awareness lock
    private bool isAwarenessLockedAtPeak = false;
    public bool IsAwarenessLockedAtPeak
    {
        get => isAwarenessLockedAtPeak;
        private set
        {
            if (isAwarenessLockedAtPeak != value)
            {
                isAwarenessLockedAtPeak = value;
                OnAwarenessPeakLockToggled?.Invoke(isAwarenessLockedAtPeak);
            }
        }
    }

    // --- Events ---
    public event Action OnAwarenessIncreaseStart;   // Player entered FOV, meter starts filling
    public event Action OnAwarenessMaxed;           // Awareness reached full (alert)
    public event Action OnAwarenessDecreaseStart;   // Player left FOV, meter starts decreasing
    public event Action OnAwarenessEmptied;         // Awareness dropped back to 0

    // NEW: lock event
    public event Action<bool> OnAwarenessPeakLockToggled;  // Invoked when lock is toggled on/off

    #region  --- Public API ---
    public bool HasLineOfSightToPlayer => PlayerTarget != null;

    public void DoubleAwarenessChangeRate() => currentAwarenessChangeRate *= 2;

    public void ResetAwarenessChangeRate() => currentAwarenessChangeRate = awarenessChangeRate;

    // NEW: Lock / Unlock methods
    public void LockAwarenessAtPeak()
    {
        AwarenessLevel = 1f; // force to max
        IsAwarenessLockedAtPeak = true;
        if (awarenessRoutine != null)
        {
            StopCoroutine(awarenessRoutine);
            awarenessRoutine = null;
        }
    }

    public void UnlockAwarenessFromPeak()
    {
        IsAwarenessLockedAtPeak = false;
        // Awareness resumes decreasing if player is not visible
        if (PlayerTarget == null)
        {
            awarenessRoutine = StartCoroutine(UpdateAwarenessLevel(increasing: false));
        }
    }
    #endregion

    // --- Unity Methods ---
    private void Awake()
    {
        fieldOfView = GetComponent<FieldOfView>();
        currentAwarenessChangeRate = awarenessChangeRate * 2;
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
        // if (IsAwarenessLockedAtPeak) return; // prevent automatic changes while locked

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
            LastKnownPlayerPosition = PlayerTarget.position;
            PlayerTarget = null;
            OnAwarenessDecreaseStart?.Invoke();
            awarenessRoutine = StartCoroutine(UpdateAwarenessLevel(increasing: false));
        }
    }

    // --- Coroutines ---
    private IEnumerator UpdateAwarenessLevel(bool increasing)
    {
        float rate = increasing ? currentAwarenessChangeRate : -currentAwarenessChangeRate;

        while (true)
        {
            if (IsAwarenessLockedAtPeak) yield break; // stop coroutine if locked

            AwarenessLevel = Mathf.Clamp01(AwarenessLevel + Time.deltaTime * rate);

            if (increasing && AwarenessLevel >= 1f)
            {
                OnAwarenessMaxed?.Invoke();
                yield break;
            }

            if (!increasing && AwarenessLevel <= 0f)
            {
                OnAwarenessEmptied?.Invoke();
                yield break;
            }

            yield return null;
        }
    }
}
