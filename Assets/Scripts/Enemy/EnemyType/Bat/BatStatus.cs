using UnityEngine;

public enum BatState
{
    Idle,
    Chase
}
[RequireComponent(typeof(Animator))]
public class BatStatus : MonoBehaviour, IEnemyHit
{
    [SerializeField] private bool _isFacingRight = false;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Rigidbody2D _rb;

    private BatState _state = BatState.Idle;
    private IAnimationController _anim;

    public BatState State => _state;

    private void Awake()
    {
        _anim = GetComponent<IAnimationController>();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Debug.Log(_state);
        if (_state == BatState.Idle) return;
        BatFlipHandler();
    }

    private void Start()
    {
        BatIdle();
    }

    public void OnHit()
    {
        _anim.PlayAnimation("Hit");
    }

    public void BatChase()
    {
        if (_state == BatState.Chase) return;
        _state = BatState.Chase;
        Debug.Log($"[BatStatus] Switch to CHASE on {name}");
        _anim?.PlayThenTransition("CeilingOut", "Flying");
    }

    public void BatIdle()
    {
        if (_state == BatState.Idle) return;
        _state = BatState.Idle;
        Debug.Log($"[BatStatus] Switch to IDLE on {name}");
        _anim?.PlayAnimation("Idle");
    }

    private void BatFlipHandler()
    {
        if (_rb.linearVelocityX > 0 && !_isFacingRight)
        {
            _isFacingRight = true;
            _spriteRenderer.flipX = !_spriteRenderer.flipX;
        }
        else if (_rb.linearVelocityX < 0 && _isFacingRight)
        {
            _isFacingRight = false;
            _spriteRenderer.flipX = !_spriteRenderer.flipX;
        }
    }
}
