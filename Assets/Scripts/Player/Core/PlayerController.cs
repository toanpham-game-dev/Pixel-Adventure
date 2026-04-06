using System.Collections;
using UnityEngine;

public enum PlayerState
{
    Normal,
    Hit,
    Dead
}

public class PlayerController : MonoBehaviour
{
    public PlayerState state = PlayerState.Normal;

    [SerializeField] private PlayerData playerData;

    [SerializeField] private string _state;
    [SerializeField] private Vector2 _respawnPosition;
    [SerializeField] private float _playerSpawnDelayTime;
    [Range(0f, 1f)][SerializeField] private float _timeScale;

    private IPlayerInput _input;
    private IAnimationController _anim;
    private IPlayerHealth _health;
    private IPlayerMovement _movement;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private AnimationController _animationController;

    public Vector2 RespawnPosition => _respawnPosition;
    public IPlayerInput Input => _input;
    public IPlayerHealth Health => _health;
    public IAnimationController Anim => _anim;
    public Rigidbody2D RB => _rb;
    public PlayerData Data => playerData;

    private void Awake()
    {
        _input = GetComponent<IPlayerInput>();
        _anim = GetComponent<IAnimationController>();
        _health = GetComponent<IPlayerHealth>();
        _movement = GetComponent<IPlayerMovement>();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animationController = GetComponent<AnimationController>();
    }

    private void Start()
    {
        Time.timeScale = _timeScale;
        _respawnPosition = transform.position;
        Appear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyHead"))
        {
            EnemyHeadHit(collision);
            return;
        }

        if (collision.CompareTag("Checkpoint"))
        {
            SaveCheckpointPosition(collision.gameObject);

            AnimationController anim = collision.GetComponent<AnimationController>();
            Collider2D col = collision.GetComponent<Collider2D>();

            if (anim != null && col != null)
            {
                anim.PlayAnimation("Checked");
                col.enabled = false;
            }
        }
    }

    public void Appear()
    {
        _input.DisableInput();
        StartCoroutine(SpawnDelay());
    }

    public void SaveCheckpointPosition(GameObject checkpoint)
    {
        _respawnPosition = checkpoint.transform.position;
    }

    public void Desappear()
    {
        _anim.PlayAnimation("Desappearing");
        _input.DisableInput();
    }

    IEnumerator SpawnDelay()
    {
        _spriteRenderer.enabled = false;

        yield return new WaitForSeconds(_playerSpawnDelayTime);

        _rb.simulated = false;

        transform.position = _respawnPosition;

        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;
        _rb.Sleep();

        _rb.simulated = true;

        _spriteRenderer.enabled = true;

        _anim.PlayAnimation("Appearing");

        _animationController.enabled = false;
    }

    public void FinishAppear()
    {
        _input.EnableInput();
        _animationController.enabled = true;
        _anim.PlayAnimation("Idle");

        state = PlayerState.Normal;
    }

    private void EnemyHeadHit(Collider2D head)
    {
        if (_rb.linearVelocity.y > 0) return;

        IEnemyHit enemy = head.GetComponentInParent<IEnemyHit>();

        if (enemy != null)
        {
            enemy.OnHit();
        }

        _movement.Jump();
    }
}