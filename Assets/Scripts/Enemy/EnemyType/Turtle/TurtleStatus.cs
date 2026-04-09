using UnityEngine;

[RequireComponent(typeof(AnimationController))]
public class TurtleStatus : EnemyStatus, IEnemyHit
{
    private TurtleBrain _turtleBrain;
    private Collider2D[] _cols;
    private Rigidbody2D _rb;
    private IAnimationController _anim;
    private bool _isIdling;

    [Header("Hit Config")]
    [SerializeField] private float _verticalForce;
    [SerializeField] private float _horizontalForce;
    [SerializeField] private GameObject _headTrigger;
    [SerializeField] private Collider2D _spikeCol;

    private void Awake()
    {
        _anim = GetComponent<IAnimationController>();
        _turtleBrain = GetComponent<TurtleBrain>();
        _cols = GetComponents<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _isIdling = true;
        _anim.PlayAnimation("Idle");
        _spikeCol.enabled = false;
    }

    private void Update()
    {
        if (State == EnemyState.Dead) return;

        if (State == EnemyState.Idle && _isIdling == false)
        {
            _anim.PlayThenTransition("Spike_In", "Idle");
            _spikeCol.enabled = false;
            _isIdling = true;
            _headTrigger.gameObject.SetActive(true);
        }

        if (State == EnemyState.Attack && _isIdling == true)
        {
            _anim.PlayThenTransition("Spike_Out", "Idle_Spike");
            _spikeCol.enabled = true;
            _isIdling = false;
            _headTrigger.gameObject.SetActive(false);
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
        _turtleBrain.enabled = false;

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
