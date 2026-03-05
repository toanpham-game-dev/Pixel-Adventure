/// <summary>
/// Base interface for all Behavior Tree nodes.
/// Every node must implement the Tick method,
/// which is called each update to evaluate the node.
/// </summary>
public interface IBehaviorNode
{
    /// <summary>
    /// Executes the node's logic for the current tick.
    /// </summary>
    /// <param name="context">Shared enemy data used by the behavior tree.</param>
    /// <param name="deltaTime">Time elapsed since the last update.</param>
    /// <returns>The result of the node evaluation.</returns>
    NodeState Tick(EnemyContext context, float deltaTime);
}
