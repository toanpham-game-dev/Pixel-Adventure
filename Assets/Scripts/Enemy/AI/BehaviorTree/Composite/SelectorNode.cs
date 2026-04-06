using System.Collections.Generic;

/// <summary>
/// Selector node that evaluates children in order.
/// Returns the first child that succeeds or is running.
/// Fails only if all children fail.
/// </summary>
public class SelectorNode : CompositeNode
{
    public SelectorNode(List<IBehaviorNode> children) : base(children) { }

    public override NodeState Tick(EnemyContext context, float deltaTime)
    {
        foreach (var child in _children)
        {
            var state = child.Tick(context, deltaTime);

            // Return immediately if a child succeeds or is still running
            if (state == NodeState.Success || state == NodeState.Running)
            {
                return state;
            }
        }

        // All children failed
        return NodeState.Failure;
    }
}