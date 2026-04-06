using System.Collections;
using UnityEngine;

public class PlayerHit : MonoBehaviour, IPlayerHit
{
    [SerializeField] private float _horizontalKnockbackForce;
    [SerializeField] private float _verticalKnockbackForce;
    [SerializeField] private float _respawnDelayTime;
    [SerializeField] private PlayerController _playerController;

    private Rigidbody2D _rb;
    private Collider2D _col;

    private void Awake()
    {
        _col = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Trap")
        {
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        if (_playerController.state != PlayerState.Normal) return;

        _playerController.state = PlayerState.Hit;

        _playerController.Anim.PlayAnimation("Hit");
        StartCoroutine(Respawn());
    }

    public static void ApplyArcKnockback(Rigidbody2D rb, Vector2 direction, float horizontalForce, float verticalForce)
    {
        direction.Normalize();
        rb.linearVelocity = Vector2.zero;
        Vector2 velocity = new Vector2(direction.x * horizontalForce, verticalForce);

        rb.linearVelocity = velocity;
    }

    private IEnumerator Respawn()
    {
        _playerController.Input.DisableInput();
        _col.enabled = false;

        _playerController.Health.DecreaseHealth(1);

        // Knockback
        float dir = Random.value < 0.5f ? -1f : 1f;
        Vector2 direction = new Vector2(dir, 0);

        ApplyArcKnockback(_rb, direction, _horizontalKnockbackForce, _verticalKnockbackForce);

        yield return new WaitForSeconds(_respawnDelayTime);

        _rb.simulated = false;

        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;

        transform.position = _playerController.RespawnPosition;

        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;

        _rb.simulated = true;

        _playerController.Anim.PlayAnimation("Appearing");

        _col.enabled = true;
    }
}
