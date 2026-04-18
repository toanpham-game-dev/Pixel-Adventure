using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour, IPlayerInput
{
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _jumpAction;

    public float Move { get; private set; }

    public bool JumpUp { get; private set; }
    public bool JumpDown { get; private set; }
    public bool JumpHeld { get; private set; }

    private void OnEnable()
    {
        // Move Input
        _moveAction.action.Enable();

        // Jump Input
        _jumpAction.action.Enable();
        _jumpAction.action.started += OnJumpStarted;
        _jumpAction.action.canceled += OnJumpCanceled;
    }

    private void OnDisable()
    {
        // Move Input
        _moveAction.action.Disable();

        // Jump Input
        _jumpAction.action.Disable();
    }

    private void Update()
    {
        JumpDown = false;
        JumpUp = false;

        float inputSystemMove = _moveAction.action.ReadValue<float>();

        Move = inputSystemMove != 0 ? inputSystemMove : InputBridge.Move;

        // Jump
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
    }

    private void OnJumpStarted(InputAction.CallbackContext ctx)
    {
        JumpDown = true;
        JumpHeld = true;
    }

    private void OnJumpCanceled(InputAction.CallbackContext ctx)
    {
        JumpUp = true;
        JumpHeld = false;
    }

    public void DisableInput()
    {
        _moveAction.action.Disable();
        _jumpAction.action.Disable();
    }

    public void EnableInput()
    {
        _moveAction.action.Enable();
        _jumpAction.action.Enable();
    }
}
