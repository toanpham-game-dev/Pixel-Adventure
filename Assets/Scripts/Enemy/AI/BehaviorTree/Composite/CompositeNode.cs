using System.Collections.Generic;

/// <summary>
/// Base class for Behavior Tree composite nodes.
/// A composite node evaluates multiple child nodes
/// using a specific execution rule.
/// </summary>
public abstract class CompositeNode : IBehaviorNode
{
    // Child nodes controlled by this composite
    protected readonly List<IBehaviorNode> _children;

    protected CompositeNode(List<IBehaviorNode> children)
    {
        _children = children;
    }

    /// <summary>
    /// Executes the composite node logic.
    /// The behavior depends on the derived type
    /// (e.g. Sequence, Selector).
    /// </summary>
    public abstract NodeState Tick(EnemyContext context, float deltaTime);
}