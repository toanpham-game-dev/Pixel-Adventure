using UnityEngine;
using System.Collections.Generic;

public class ChickenBrain : EnemyBrain
{
    [SerializeField] private LayerMask _targetMask;

    public Transform Target => target;

    protected override IBehaviorNode CreateBehaviorTree()
    {
        return new SelectorNode(new List<IBehaviorNode>
        {
            // Dead
            new SequenceNode(new List<IBehaviorNode>
            {
                new ConditionIsDead(),
                new ActionDie()
            }),

            // Chase
            new SequenceNode(new List<IBehaviorNode>
            {
                new ConditionCanSeeTarget(_targetMask),
                new ActionMoveToTargetGround()
            }),

            // Idle
            new ActionIdle()
        });
    }
}
