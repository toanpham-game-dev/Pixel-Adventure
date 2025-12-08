using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Data
    [SerializeField] private PlayerData playerData;

    [SerializeField] private string _state;
    [Range(0f, 1f)][SerializeField] private float _timeScale;

    // Components
    private IPlayerInput _input;
    private IAnimationController _anim;
    private IPlayerMovement _movement;
    private Rigidbody2D _rb;

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
    }
}
