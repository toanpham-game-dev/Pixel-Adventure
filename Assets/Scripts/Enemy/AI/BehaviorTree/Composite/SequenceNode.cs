using System.Collections.Generic;

/// <summary>
/// Sequence node that evaluates children in order.
/// Fails if any child fails, runs if a child is running,
/// and succeeds only when all children succeed.
/// </summary>
public class SequenceNode : CompositeNode
{
    public SequenceNode(List<IBehaviorNode> children) : base(children) { }

    public override NodeState Tick(EnemyContext context, float deltaTime)
    {
        bool anyRunning = false;

        foreach (var child in _children)
        {
            var state = child.Tick(context, deltaTime);

            // Fail immediately if a child fails
            if (state == NodeState.Failure)
            {
                return NodeState.Failure;
            }

            // Sequence continues running if a child is still running
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