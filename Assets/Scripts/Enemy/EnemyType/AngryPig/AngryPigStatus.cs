using UnityEngine;

public enum AngryPigState
{
    Walk,
    Run,
    Dead
}

[RequireComponent(typeof(Animator))]
public class AngryPigStatus : MonoBehaviour, IEnemyHit
{
    [Header("Hit Config")]
    [SerializeField] private int hitsToDie;

    private int _currentHits;
    private AngryPigState _state = AngryPigState.Walk;
    private IAnimationController _anim;

    public AngryPigState State => _state;

    private void Awake()
    {
        _anim = GetComponent<IAnimationController>();
    }

    private void Start()
    {
        _anim.PlayAnimation("Walk");
    }

    public void OnHit()
    {
        if (_state == AngryPigState.Dead) return;

        _currentHits++;

        if (_currentHits == 1)
        {
            _state = AngryPigState.Run;
            _anim.PlayThenTransition("Hit", "Run");
        }
        else if (_currentHits >= hitsToDie)
        {
            _state = AngryPigState.Dead;
            _anim.PlayAnimation("Dead");
        }
    }
}
