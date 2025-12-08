using UnityEngine;

public interface IAnimationController
{
    void PlayAnimation(string animationName);
    void PlayThenTransition(string firstAnim, string nextAnim);
}
