using UnityEngine;

/// <summary>
/// Defines basic animation control behaviors for a character or entity.
/// </summary>
public interface IAnimationController
{
    /// <summary>
    /// Plays an animation immediately.
    /// </summary>
    void PlayAnimation(string animationName);

    /// <summary>
    /// Plays one animation, then transitions to another when it finishes.
    /// </summary>
    void PlayThenTransition(string firstAnim, string nextAnim);
}