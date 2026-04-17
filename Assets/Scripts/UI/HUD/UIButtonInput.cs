using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum ButtonType
    {
        MoveLeft,
        MoveRight,
        Jump
    }

    public ButtonType type;

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (type)
        {
            case ButtonType.MoveLeft:
                InputBridge.Move = -1;
                break;

            case ButtonType.MoveRight:
                InputBridge.Move = 1;
                break;

            case ButtonType.Jump:
                InputBridge.JumpPressed = true;
                InputBridge.JumpHeld = true;
                break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        switch (type)
        {
            case ButtonType.MoveLeft:
            case ButtonType.MoveRight:
                InputBridge.Move = 0;
                break;

            case ButtonType.Jump:
                InputBridge.JumpReleased = true;
                InputBridge.JumpHeld = false;
                break;
        }
    }
}