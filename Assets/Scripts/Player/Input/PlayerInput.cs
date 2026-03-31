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
        Move = _moveAction.action.ReadValue<float>();
    }

    void LateUpdate()
    {
        // Make sure to reset JumpDown and JumpUp each frame
        JumpDown = false;
        JumpUp = false;
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
