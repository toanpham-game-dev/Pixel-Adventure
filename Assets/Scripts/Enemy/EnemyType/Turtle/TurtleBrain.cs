using System.Collections.Generic;
using UnityEngine;

public class TurtleBrain : EnemyBrain
{
    [SerializeField] private float _idleTime;
    [SerializeField] private float _spikeTime;
    protected override IBehaviorNode CreateBehaviorTree()
    {
        return new SelectorNode(new List<IBehaviorNode>
        {
            new SequenceNode(new List<IBehaviorNode>
            {
                new ConditionIsDead(),
                new ActionDie()
            }),

            new ActionTurtleIdle(_idleTime, _spikeTime)
        });
    }
}
