using UnityEngine;

public class BlueBirdStatus : EnemyStatus, IEnemyHit
{
    [SerializeField] private IAnimationController _anim;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Collider2D _col;
    [SerializeField] private BlueBirdBrain _blueBirdBrain;
    [Header("Hit Config")]
    [SerializeField] private GameObject _headTrigger;
    [SerializeField] private float _horizontalForce;
    [SerializeField] private float _verticalForce;

    private void Awake()
    {
        _anim = GetComponent<AnimationController>();
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
        _blueBirdBrain = GetComponent<BlueBirdBrain>();
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
        _col.enabled = false;
        _blueBirdBrain.enabled = false;

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
