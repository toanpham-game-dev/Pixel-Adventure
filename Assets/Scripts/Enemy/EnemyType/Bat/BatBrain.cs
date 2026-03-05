using UnityEngine;
using System.Collections.Generic;

public class BatBrain : EnemyBrain
{
    [SerializeField] private LayerMask _targetMask;

    protected override IBehaviorNode CreateBehaviorTree()
    {
        return new SelectorNode(new List<IBehaviorNode>
        {
            // 1) If bat can see the target -> chase
            new SequenceNode(new List<IBehaviorNode>
            {
                new ConditionCanSeeTarget(_targetMask),
                new BatChaseNode()
            }),

            // 2) Default behavior: idle
            new BatIdleNode()
        });
    }
}
