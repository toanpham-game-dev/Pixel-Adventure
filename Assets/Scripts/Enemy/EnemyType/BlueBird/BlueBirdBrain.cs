using System.Collections.Generic;
using UnityEngine;

public class BlueBirdBrain : EnemyBrain
{
    [Header("Patrol Settings")]
    [SerializeField] private float _wallCheckDistance;
    [SerializeField] private float _speed;
    [SerializeField] private LayerMask _wallMask;
    [SerializeField] private float _idleDuration;
    [SerializeField] private Vector2 _wallCheckSize;

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

            // Idle
            new SequenceNode(new List<IBehaviorNode>
            {
                new ConditionCanIdle(),
                new ActionPigIdle(_idleDuration)
            }),

            //Patrol
            new ActionFlyPatrol(_speed, _wallCheckDistance,_wallCheckSize, _wallMask)
        });
    }
}
