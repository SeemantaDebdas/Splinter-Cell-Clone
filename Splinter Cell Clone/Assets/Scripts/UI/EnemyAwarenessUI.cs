using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAwarenessUI : MonoBehaviour
{
    [SerializeField] Awareness awareness;
    [SerializeField] Slider questionSlider;
    [SerializeField] Image exclamationImage;

    bool awarenessPeakLocked = false;

    void OnEnable()
    {
        awareness.OnAwarenessIncreaseStart += Awareness_OnAwarenessIncreaseStart;
        awareness.OnAwarenessEmptied += Awareness_OnAwarenessEmptied;
        awareness.OnAwarenessMaxed += Awareness_OnAwarenessMaxed;
        awareness.OnAwarenessPeakLockToggled += Awareness_OnAwarenessPeakLockToggled;

        questionSlider.gameObject.SetActive(false);
        exclamationImage.rectTransform.localScale = Vector3.zero;
    }

    void OnDisable()
    {
        awareness.OnAwarenessIncreaseStart -= Awareness_OnAwarenessIncreaseStart;
        awareness.OnAwarenessEmptied -= Awareness_OnAwarenessEmptied;
        awareness.OnAwarenessMaxed -= Awareness_OnAwarenessMaxed;
        awareness.OnAwarenessPeakLockToggled -= Awareness_OnAwarenessPeakLockToggled;
    }

    void Awareness_OnAwarenessMaxed()
    {
        if (awarenessPeakLocked)
            return;

        questionSlider.gameObject.SetActive(false);

        // DOTween pop animation
        Sequence popSeq = DOTween.Sequence();
        popSeq.Append(exclamationImage.rectTransform.DOScale(new Vector3(2, 2, 2), 0.3f).SetEase(Ease.OutBack))
              .Append(exclamationImage.rectTransform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack));
    }

    void Awareness_OnAwarenessIncreaseStart()
    {
        if (awarenessPeakLocked)
            return;

        questionSlider.gameObject.SetActive(true);
    }

    void Awareness_OnAwarenessEmptied()
    {
        if (awarenessPeakLocked)
            return;

        questionSlider.gameObject.SetActive(false);
    }

    void Awareness_OnAwarenessPeakLockToggled(bool locked)
    {
        awarenessPeakLocked = locked;
    }

    void Update()
    {
        if (!awareness.gameObject.activeSelf || awarenessPeakLocked)
            return;

        questionSlider.value = awareness.AwarenessLevel;
    }
}
