using System.Collections.Generic;

/// <summary>
/// A Sequence node evaluates its child nodes in order on every tick.
/// - Returns Failure immediately if any child fails.
/// - Returns Running if a child is still running.
/// - Returns Success only if all children succeed.
/// 
/// This implementation is stateless and re-evaluates
/// children from the beginning each tick.
/// </summary>
public class SequenceNode : CompositeNode
{
    /// <summary>
    /// Creates a sequence node with the given child nodes.
    /// </summary>
    /// <param name="children">Child behavior nodes.</param>
    public SequenceNode(List<IBehaviorNode> children) : base(children) { }

    /// <summary>
    /// Executes the sequence logic by ticking children in order.
    /// </summary>
    public override NodeState Tick(EnemyContext context, float deltaTime)
    {
        bool anyRunning = false;

        foreach (var child in _children)
        {
            var state = child.Tick(context, deltaTime);

            // If any child fails, the whole sequence fails
            if (state == NodeState.Failure)
            {
                return NodeState.Failure;
            }

            // If a child is running, the sequence stays running
            if (state == NodeState.Running)
            {
                anyRunning = true;
                break;
            }
        }

        if (anyRunning)
            return NodeState.Running;

        // All children succeeded
        return NodeState.Success;
    }
}
