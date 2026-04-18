using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ChickenStatus : EnemyStatus, IEnemyHit
{
    [SerializeField] private bool _isFacingRight = false;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;
    private Collider2D[] _cols;
    private IAnimationController _anim;

    [SerializeField] private ChickenBrain _chickenBrain;
    [SerializeField] private float _verticalForce;
    [SerializeField] private float _horizontalForce;
    [SerializeField] private GameObject _headTrigger;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _anim = GetComponent<IAnimationController>();
        _chickenBrain = GetComponent<ChickenBrain>();
        _cols = GetComponents<Collider2D>();
    }

    private void Start()
    {
        State = EnemyState.Idle;
    }

    private void Update()
    {
        if (State == EnemyState.Dead)
            return;

        if (State == EnemyState.Idle)
        {
            _anim.PlayAnimation("Idle");
        }

        if (State == EnemyState.Chase)
        {
            _anim.PlayAnimation("Run");
        }
        HandleFlip();
    }

    public void OnHit()
    {
        State = EnemyState.Dead;
        OnDead();
        _anim.PlayAnimation("Hit");
        _headTrigger.SetActive(false);
    }

    private void HandleFlip()
    {
        if (_chickenBrain == null || _chickenBrain.Target == null)
            return;

        float dir = _chickenBrain.Target.position.x - transform.position.x;

        if (Mathf.Abs(dir) < 0.1f)
            return;

        bool targetRight = dir > 0;

        if (targetRight != _isFacingRight)
            Flip();
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        _spriteRenderer.flipX = !_spriteRenderer.flipX;
    }

    private void OnDead()
    {
        foreach (var col in _cols)
        {
            col.enabled = false;
        }
        _chickenBrain.enabled = false;

        float dir = Random.value < 0.5f ? -1f : 1f;
        Vector2 direction = new Vector2(dir, 0);

        ApplyArcKnockback(_rb, direction, _horizontalForce, _verticalForce);
    }

    public static void ApplyArcKnockback(Rigidbody2D rb, Vector2 direction, float horizontalForce, float verticalForce)
    {
        direction.Normalize();
        rb.linearVelocity = Vector2.zero;
        Vector2 velocity = new Vector2(direction.x * horizontalForce, verticalForce);

        rb.linearVelocity = velocity;
    }
}