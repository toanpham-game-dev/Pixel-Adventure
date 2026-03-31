using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Data
    [SerializeField] private PlayerData playerData;

    [SerializeField] private string _state;
    [SerializeField] private Vector2 _respawnPosition;
    [SerializeField] private float _playerSpawnDelayTime;
    [Range(0f, 1f)][SerializeField] private float _timeScale;

    // Components
    private IPlayerInput _input;
    private IAnimationController _anim;
    private IPlayerMovement _movement;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private AnimationController _animationController;

    // Readonly exposes
    public IPlayerInput Input => _input;
    public IAnimationController Anim => _anim;
    public IPlayerMovement Movement => _movement;
    public Rigidbody2D RB => _rb;
    public PlayerData Data => playerData;

    private void Awake()
    {
        // Cache components
        _input = GetComponent<IPlayerInput>();
        _anim = GetComponent<IAnimationController>();
        _movement = GetComponent<IPlayerMovement>();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animationController = GetComponent<AnimationController>();
    }

    private void Start()
    {
        Time.timeScale = _timeScale;
        Appear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Checkpoint")
        {
            SaveCheckpointPosition(collision.gameObject);
            AnimationController anim = collision.gameObject.GetComponent<AnimationController>();
            if (anim != null) 
            {
                anim.PlayAnimation("Checked");
            }
            Debug.Log("Checkpoint saved!");
        }
    }

    public void Appear()
    {
        _respawnPosition = gameObject.transform.position;
        _input.DisableInput();
        StartCoroutine(SpawnDelay());
    }

    public void SaveCheckpointPosition(GameObject Checkpoint)
    {
        _respawnPosition = Checkpoint.transform.position;
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

        _spriteRenderer.enabled = true;
        _anim.PlayAnimation("Appearing");
        _animationController.enabled = false;
    }

    public void FinishAppear()
    {
        _input.EnableInput();
        _animationController.enabled = true;
        _anim.PlayAnimation("Idle");
    }
}
