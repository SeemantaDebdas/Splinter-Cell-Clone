using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAwarenessUI : MonoBehaviour
{
    [SerializeField] Awareness awareness;
    [SerializeField] Slider questionSlider;
    [SerializeField] Image exclamationImage;

    void OnEnable()
    {
        awareness.OnAwarenessIncreaseStart += Awareness_OnAwarenessIncreaseStart;
        awareness.OnAwarenessEmptied += Awareness_OnAwarenessEmptied;
        awareness.OnAwarenessMaxed += Awareness_OnAwarenessMaxed;

        questionSlider.gameObject.SetActive(false);
        exclamationImage.rectTransform.localScale = Vector3.zero;
    }

    void OnDisable()
    {
        awareness.OnAwarenessIncreaseStart -= Awareness_OnAwarenessIncreaseStart;
        awareness.OnAwarenessEmptied -= Awareness_OnAwarenessEmptied;
        awareness.OnAwarenessMaxed -= Awareness_OnAwarenessMaxed;

    }

    void Awareness_OnAwarenessMaxed()
    {
        questionSlider.gameObject.SetActive(false);

        // DOTween pop animation
        Sequence popSeq = DOTween.Sequence();
        popSeq.Append(exclamationImage.rectTransform.DOScale(new Vector3(2, 2, 2), 0.3f).SetEase(Ease.OutBack))
              .Append(exclamationImage.rectTransform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack));
    }

    void Awareness_OnAwarenessIncreaseStart()
    {
        questionSlider.gameObject.SetActive(true);
    }

    void Awareness_OnAwarenessEmptied()
    {
        questionSlider.gameObject.SetActive(false);
    }


    void Update()
    {
        if (!awareness.gameObject.activeSelf)
            return;

        questionSlider.value = awareness.AwarenessLevel;
    }
}
