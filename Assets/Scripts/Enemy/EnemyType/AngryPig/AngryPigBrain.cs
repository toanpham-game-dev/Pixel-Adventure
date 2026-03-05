using UnityEngine;
using System.Collections.Generic;

public class AngryPigBrain : EnemyBrain
{
    [Header("Patrol Settings")]
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private float _wallCheckDistance;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private LayerMask _groundMask;

    protected override IBehaviorNode CreateBehaviorTree()
    {
        return new SelectorNode(new List<IBehaviorNode>
        {
            new SequenceNode(new List<IBehaviorNode>
            {
                new PigConditionIsDead(),
                new PigDieNode()
            }),

            new PigPatrolGroundNode(transform, _groundCheckDistance, _wallCheckDistance, _walkSpeed, _runSpeed, _groundMask)
        });
    }
}
