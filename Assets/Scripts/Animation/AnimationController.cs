using UnityEngine;

public class AnimationController : MonoBehaviour, IAnimationController
{
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayAnimation(string animationName)
    {
        _animator.Play(animationName);
    }
}
