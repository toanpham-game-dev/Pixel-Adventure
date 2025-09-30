using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Data
    [SerializeField] private PlayerData playerData;

    // Components
    private IPlayerInput _input;
    private IAnimationController _anim;
    private IPlayerMovement _movement;
    private Rigidbody2D _rb;

    // State Machine
    private PlayerStateMachine _stateMachine;
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }

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

        // State machine & states
        _stateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, _stateMachine);
        MoveState = new PlayerMoveState(this, _stateMachine);
    }

    private void Start()
    {
        _stateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }
}
