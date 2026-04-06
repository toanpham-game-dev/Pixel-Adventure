using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AngryPigStatus : EnemyStatus, IEnemyHit
{
    [SerializeField] private AngryPigBrain _angryPigBrain;
    [Header("Hit Config")]
    [SerializeField] private int hitsToDie;
    [SerializeField] private float _verticalForce;
    [SerializeField] private float _horizontalForce;
    [SerializeField] private GameObject _headTrigger;

    private Collider2D _col;
    private Rigidbody2D _rb;

    private int _currentHits;
    private IAnimationController _anim;

    private void Awake()
    {
        _anim = GetComponent<IAnimationController>();
        _angryPigBrain = GetComponent<AngryPigBrain>();
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
        if (State == EnemyState.Move && _currentHits != 1)
        {
            _anim.PlayAnimation("Walk");
        }
    }

    public void OnHit()
    {
        if (State == EnemyState.Dead) return;

        _currentHits++;

        if (_currentHits == 1)
        {
            State = EnemyState.Chase;
            _anim.PlayThenTransition("Hit", "Run");
            _angryPigBrain.WalkSpeed = _angryPigBrain.WalkSpeed * 2;
        }
        else if (_currentHits >= hitsToDie)
        {
            State = EnemyState.Dead;
            OnDead();
            _anim.PlayAnimation("Dead");
            _headTrigger.SetActive(false);
        }
    }

    private void OnDead()
    {
        _col.enabled = false;
        _angryPigBrain.enabled = false;

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
