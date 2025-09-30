using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour, IPlayerInput
{
    public InputActionReference moveAction;

    public float Move { get; private set; }

    public bool Jump { get; private set; }

    private void OnEnable()
    {
        moveAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
    }

    private void Update()
    {
        Move = moveAction.action.ReadValue<float>();
    }
}
