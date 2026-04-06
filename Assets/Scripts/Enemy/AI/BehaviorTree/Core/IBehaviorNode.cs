/// <summary>
/// Base interface for all Behavior Tree nodes.
/// Each node must implement the Tick method.
/// </summary>
public interface IBehaviorNode
{
    /// <summary>
    /// Executes the node logic and returns its state.
    /// </summary>
    NodeState Tick(EnemyContext context, float deltaTime);
}