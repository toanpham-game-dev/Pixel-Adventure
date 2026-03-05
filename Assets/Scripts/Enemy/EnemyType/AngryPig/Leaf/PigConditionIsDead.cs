/// <summary>
/// Behavior Tree condition node that checks whether the AngryPig has entered the Dead state.
/// Returns Success if the pig is dead, otherwise Failure.
/// </summary>
public class PigConditionIsDead : IBehaviorNode
{
    public NodeState Tick(EnemyContext context, float deltaTime)
    {
        // Retrieve the AngryPigStatus component which stores the pig's current state.
        var status = context.Self.GetComponent<AngryPigStatus>();

        if (status != null && status.State == AngryPigState.Dead)
            return NodeState.Success;

        return NodeState.Failure;
    }
}