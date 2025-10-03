using UnityEngine;

public static class AnimatorExtension
{
    /// <summary>
    /// Returns the normalized time of the current Animator state on a specific layer.
    /// Normalized time: 0 = start, 1 = end, 2 = second loop, etc.
    /// </summary>
    public static float GetNormalizedTime(this Animator animator, int layer = 0)
    {
        if (animator == null || animator.runtimeAnimatorController == null)
            return 0f;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layer);
        return stateInfo.normalizedTime;
    }

    /// <summary>
    /// Returns true if the Animator is currently in a state with the given name.
    /// </summary>
    public static bool IsInState(this Animator animator, string stateName, int layer = 0)
    {
        if (animator == null || animator.runtimeAnimatorController == null)
            return false;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layer);
        return stateInfo.IsName(stateName);
    }

    public static bool HasAnimationEnded(this Animator animator, string stateName, float thresholdTime = 1f, int layer = 0)
    {
        float normalizedTime = animator.GetNormalizedTime(layer);

        if (animator.IsInState(stateName, layer) && normalizedTime >= thresholdTime)
        {
            return true;
        }

        return false;
    }
}