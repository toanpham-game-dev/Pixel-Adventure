using UnityEngine;

public class FatBirdStatus : EnemyStatus, IEnemyHit
{
    [SerializeField] private IAnimationController _anim;
    [SerializeField] private GameObject _headTrigger;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Collider2D[] _cols;
    [SerializeField] private FatBirdBrain _fatBirdBrain;
    [SerializeField] private float _horizontalForce;
    [SerializeField] private float _verticalForce;

    private void Awake()
    {
        _anim = GetComponent<AnimationController>();
        _cols = GetComponents<Collider2D>();
        _fatBirdBrain = GetComponent<FatBirdBrain>();
        _rb = GetComponent<Rigidbody2D>();
    }

    public bool IsDiving = false;

    private void Update()
    {
        if (State == EnemyState.Dead) return;

        if (State == EnemyState.Attack)
        {
            _anim.PlayAnimation("Fall");
        }

        if (State == EnemyState.IsGrounded)
        {
            _anim.PlayThenTransition("Ground", "Idle");
        }
    }

    public void OnHit()
    {
        State = EnemyState.Dead;
        OnDead();
        _anim.PlayAnimation("Hit");
        _rb.gravityScale = 1;
        _headTrigger.SetActive(false);
    }

    private void OnDead()
    {
        foreach (var col in _cols)
        {
            col.enabled = false;
        }
        _fatBirdBrain.enabled = false;

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
