using System.Collections.Generic;

/// <summary>
/// Base class for all composite nodes in a Behavior Tree.
/// A composite node contains multiple child nodes and
/// controls how they are evaluated.
/// </summary>
public abstract class CompositeNode : IBehaviorNode
{
    /// <summary>
    /// Child nodes managed by this composite node.
    /// </summary>
    protected readonly List<IBehaviorNode> _children;

    /// <summary>
    /// Creates a composite node with the given child nodes.
    /// </summary>
    /// <param name="children">List of child behavior nodes.</param>
    protected CompositeNode(List<IBehaviorNode> children)
    {
        _children = children;
    }

    /// <summary>
    /// Evaluates the composite node logic.
    /// Each derived composite (Sequence, Selector, etc.)
    /// defines its own execution rules.
    /// </summary>
    /// <param name="context">Shared enemy data used by the behavior tree.</param>
    /// <param name="deltaTime">Time elapsed since the last update.</param>
    /// <returns>The result of the node evaluation.</returns>
    public abstract NodeState Tick(EnemyContext context, float deltaTime);
}
