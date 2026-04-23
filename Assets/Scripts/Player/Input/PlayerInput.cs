using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour, IPlayerInput
{
    [Header("Input Actions (PC)")]
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _jumpAction;

    public float Move { get; private set; }

    public bool JumpUp { get; private set; }
    public bool JumpDown { get; private set; }
    public bool JumpHeld { get; private set; }

    private bool _jumpPressedBuffer;
    private bool _jumpReleasedBuffer;

    private void OnEnable()
    {
#if !UNITY_ANDROID
        // Enable Action Map
        _moveAction.action.actionMap.Enable();
        _jumpAction.action.actionMap.Enable();

        _moveAction.action.Enable();
        _jumpAction.action.Enable();

        _jumpAction.action.started += OnJumpStarted;
        _jumpAction.action.canceled += OnJumpCanceled;
#endif
    }

    private void OnDisable()
    {
#if !UNITY_ANDROID
        _jumpAction.action.started -= OnJumpStarted;
        _jumpAction.action.canceled -= OnJumpCanceled;

        _moveAction.action.Disable();
        _jumpAction.action.Disable();
#endif
    }

    private void Update()
    {
        JumpDown = false;
        JumpUp = false;

#if UNITY_ANDROID
        // ================= MOBILE =================
        Move = InputBridge.Move;

        if (InputBridge.JumpPressed)
        {
            JumpDown = true;
            JumpHeld = true;
            InputBridge.JumpPressed = false;
        }

        if (InputBridge.JumpReleased)
        {
            JumpUp = true;
            JumpHeld = false;
            InputBridge.JumpReleased = false;
        }

#else
        // ================= PC =================
        Move = _moveAction.action.ReadValue<float>();

        if (_jumpPressedBuffer)
        {
            JumpDown = true;
            JumpHeld = true;
            _jumpPressedBuffer = false;
        }

        if (_jumpReleasedBuffer)
        {
            JumpUp = true;
            JumpHeld = false;
            _jumpReleasedBuffer = false;
        }
#endif
    }

#if !UNITY_ANDROID
    private void OnJumpStarted(InputAction.CallbackContext ctx)
    {
        _jumpPressedBuffer = true;
    }

    private void OnJumpCanceled(InputAction.CallbackContext ctx)
    {
        _jumpReleasedBuffer = true;
    }
#endif

    public void DisableInput()
    {
#if !UNITY_ANDROID
        _moveAction.action.Disable();
        _jumpAction.action.Disable();
#endif
    }

    public void EnableInput()
    {
#if !UNITY_ANDROID
        _moveAction.action.Enable();
        _jumpAction.action.Enable();
#endif
    }
}