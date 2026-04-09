using UnityEngine;

public class DuckStatus : EnemyStatus, IEnemyHit
{
    private DuckBrain _duckBrain;
    private Collider2D[] _cols;
    private Rigidbody2D _rb;
    private IAnimationController _anim;

    [Header("Hit Config")]
    [SerializeField] private float _verticalForce;
    [SerializeField] private float _horizontalForce;
    [SerializeField] private GameObject _headTrigger;

    private void Awake()
    {
        _anim = GetComponent<IAnimationController>();
        _duckBrain = GetComponent<DuckBrain>();
        _cols = GetComponents<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (State == EnemyState.Idle)
        {
            _anim.PlayAnimation("Idle");
            return;
        }

        if (State == EnemyState.Move)
        {
            float vy = _rb.linearVelocity.y;

            if (Mathf.Abs(_rb.linearVelocity.x) < 0.01f && vy == 0)
            {
                _anim.PlayThenTransition("Jump_Anticipation", "Jump");
            }
            else if (vy < -0.1f)
            {
                _anim.PlayAnimation("Fall");
            }
        }
    }

    public void OnHit()
    {
        State = EnemyState.Dead;
        OnDead();
        _anim.PlayAnimation("Hit");
        _headTrigger.SetActive(false);
    }

    private void OnDead()
    {
        foreach (var col in _cols)
        {
            col.enabled = false;
        }
        _duckBrain.enabled = false;

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
