using UnityEngine;

public class MushroomStatus : EnemyStatus, IEnemyHit
{
    [SerializeField] private MushroomBrain _mushroomBrain;
    private Collider2D _col;
    private Rigidbody2D _rb;
    private IAnimationController _anim;

    [Header("Hit Config")]
    [SerializeField] private float _verticalForce;
    [SerializeField] private float _horizontalForce;
    [SerializeField] private GameObject _headTrigger;

    private void Awake()
    {
        _anim = GetComponent<IAnimationController>();
        _mushroomBrain = GetComponent<MushroomBrain>();
        _col = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _anim.PlayAnimation("Walk");
        State = EnemyState.Move;
    }

    private void Update()
    {
        if (State == EnemyState.Idle)
        {
            _anim.PlayAnimation("Idle");
        }

        if (State == EnemyState.Move)
        {
            _anim.PlayAnimation("Walk");
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
        _col.enabled = false;
        _mushroomBrain.enabled = false;

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
