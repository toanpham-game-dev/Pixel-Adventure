using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour, IAnimationController
{
    [SerializeField] private Animator _animator;

    // Coroutine used for delayed animation transitions
    private Coroutine _transitionCoroutine;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Play an animation immediately.
    /// Any running transition coroutine will be stopped.
    /// </summary>
    public void PlayAnimation(string animationName)
    {
        if (!enabled) return;

        if (_transitionCoroutine != null)
        {
            StopCoroutine(_transitionCoroutine);
            _transitionCoroutine = null;
        }

        _animator.Play(animationName);
    }

    /// <summary>
    /// Play one animation, then automatically transition to another
    /// after the first animation finishes.
    /// </summary>
    public void PlayThenTransition(string firstAnim, string nextAnim)
    {
        if (!enabled) return;

        if (_transitionCoroutine != null)
        {
            StopCoroutine(_transitionCoroutine);
        }

        _transitionCoroutine = StartCoroutine(
            PlayThenTransitionRoutine(firstAnim, nextAnim)
        );
    }

    /// <summary>
    /// Coroutine that waits for the first animation to finish
    /// before playing the next animation.
    /// </summary>
    private IEnumerator PlayThenTransitionRoutine(string firstAnim, string nextAnim)
    {
        _animator.Play(firstAnim);

        // Wait one frame so Animator can update its state
        yield return null;

        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        float waitTime = stateInfo.length;

        yield return new WaitForSeconds(waitTime);

        _animator.Play(nextAnim);
        _transitionCoroutine = null;
    }
}