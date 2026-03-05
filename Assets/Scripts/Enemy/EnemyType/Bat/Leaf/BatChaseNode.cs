/// <summary>
/// Wrapper node for bat chasing behavior:
/// - Sets bat state to Chase
/// - Delegates movement logic to ActionMoveToTarget (A* chasing)
/// </summary>
public class BatChaseNode : IBehaviorNode
{
    private readonly ActionMoveToTarget _moveNode = new ActionMoveToTarget();

    public NodeState Tick(EnemyContext context, float deltaTime)
    {
        var status = context.Self.GetComponent<BatStatus>();
        if (status != null)
        {
            status.BatChase();
        }

        _moveNode.Tick(context, deltaTime);
        return NodeState.Running;
    }
}
