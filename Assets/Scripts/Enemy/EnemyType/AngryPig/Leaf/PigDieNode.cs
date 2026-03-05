using UnityEngine;

/// <summary>
/// Behavior Tree action node responsible for handling the pig's death behavior.
/// - Stops movement
/// - Disables collider
/// - Plays death animation (handled externally)
/// - Destroys the pig after a short delay
/// </summary>
public class PigDieNode : IBehaviorNode
{
    private bool _initialized; // Ensures the death logic only runs once
    private float _timer = 1.5f;

    public NodeState Tick(EnemyContext context, float deltaTime)
    {
        var rb = context.Self.GetComponent<Rigidbody2D>();
        var col = context.Self.GetComponent<Collider2D>();

        // Initialization block runs only on the first tick.
        if (!_initialized)
        {
            // Stop movement immediately
            if (rb) rb.linearVelocity = Vector2.zero;

            // Disable collider to prevent further interactions
            if (col) col.enabled = false;
            _initialized = true;
        }

        // Countdown until destruction
        _timer -= deltaTime;

        // If the timer has expired, destroy the pig and return Success
        if (_timer <= 0f)
        {
            Object.Destroy(context.Self.gameObject);
            return NodeState.Success;
        }

        return NodeState.Running;
    }
}
