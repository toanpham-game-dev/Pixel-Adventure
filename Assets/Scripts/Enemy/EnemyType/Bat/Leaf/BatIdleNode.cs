using UnityEngine;

/// <summary>
/// Keeps the bat idle: stops movement and sets BatState to Idle.
/// </summary>
public class BatIdleNode : IBehaviorNode
{
    public NodeState Tick(EnemyContext context, float deltaTime)
    {
        // Stop movement if mover exists
        context.Mover?.Stop();

        // Set state to Idle for animation
        var status = context.Self.GetComponent<BatStatus>();
        if (status != null)
        {
            status.BatIdle();
        }

        // Idle is a continuous state -> Running
        return NodeState.Running;
    }
}
