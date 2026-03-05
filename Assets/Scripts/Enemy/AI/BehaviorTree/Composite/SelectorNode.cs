using System.Collections.Generic;

/// <summary>
/// A Selector node evaluates its child nodes from left to right.
/// - Returns Success as soon as one child succeeds.
/// - Returns Running if a child is still running.
/// - Returns Failure only if all children fail.
/// 
/// This implementation is stateless and re-evaluates
/// all children from the beginning on each tick.
/// </summary>
public class SelectorNode : CompositeNode
{
    /// <summary>
    /// Creates a selector node with the given child nodes.
    /// </summary>
    /// <param name="children">Child behavior nodes.</param>
    public SelectorNode(List<IBehaviorNode> children) : base(children) { }

    /// <summary>
    /// Executes the selector logic by ticking children in order.
    /// </summary>
    public override NodeState Tick(EnemyContext context, float deltaTime)
    {
        foreach (var child in _children)
        {
            var state = child.Tick(context, deltaTime);

            // First child that is not Failure determines the result
            if (state == NodeState.Success || state == NodeState.Running)
            {
                return state;
            }
        }

        // All children failed
        return NodeState.Failure;
    }
}
