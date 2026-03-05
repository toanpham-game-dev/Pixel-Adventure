using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour, IAnimationController
{
    [SerializeField] private Animator _animator;

    private Coroutine _transitionCoroutine;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Plays the specified animation immediately.
    /// </summary>
    /// <param name="animationName">Animation state name in Animator.</param>
    public void PlayAnimation(string animationName)
    {
        if (_transitionCoroutine != null)
        {
            StopCoroutine(_transitionCoroutine);
            _transitionCoroutine = null;
        }

        _animator.Play(animationName);
    }

    /// <summary>
    /// Plays the first animation, then transitions to the next.
    /// </summary>
    /// <param name="firstAnim">First animation state name.</param>
    /// <param name="nextAnim">Next animation state name.</param>
    public void PlayThenTransition(string firstAnim, string nextAnim)
    {
        if (_transitionCoroutine != null)
        {
            StopCoroutine(_transitionCoroutine);
        }

        StartCoroutine(PlayThenTransitionRoutine(firstAnim, nextAnim));
    }

    private IEnumerator PlayThenTransitionRoutine(string firstAnim, string nextAnim)
    {
        _animator.Play(firstAnim);
        yield return null;

        // Wait for the current animation to finish
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        float waitTime = stateInfo.length;

        yield return new WaitForSeconds(waitTime);

        _animator.Play(nextAnim);
        _transitionCoroutine = null;
    }
}
