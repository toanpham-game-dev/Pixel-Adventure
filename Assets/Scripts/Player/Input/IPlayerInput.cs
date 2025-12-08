using UnityEngine;

public interface IPlayerInput
{
    float Move { get; }
    bool JumpUp { get; }
    bool JumpDown { get; }
    bool JumpHeld { get; }
}
