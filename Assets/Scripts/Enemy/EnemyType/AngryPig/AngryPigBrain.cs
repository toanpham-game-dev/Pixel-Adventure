using UnityEngine;
using System.Collections.Generic;

public class AngryPigBrain : EnemyBrain
{
    [Header("Patrol Settings")]
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private float _wallCheckDistance;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _idleDuration;

    public float WalkSpeed
    {
        get { return _walkSpeed; }
        set { _walkSpeed = value; }
    }

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

            // Angry
            new SequenceNode(new List<IBehaviorNode>
            {
                new ConditionIsAngry(),
                new ActionAngryPatrol(_groundCheckDistance,_wallCheckDistance,_walkSpeed,_groundMask)
            }),

            // Idle
            new SequenceNode(new List<IBehaviorNode>
            {
                new ConditionCanIdle(),
                new ActionPigIdle(_idleDuration)
            }),

            // Patrol
            new ActionPatrol(_groundCheckDistance,_wallCheckDistance,_walkSpeed,_groundMask)
        });
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 pos = transform.position;

        float dir = transform.localScale.x > 0 ? 1 : -1;

        Vector3 forward = new Vector3(dir, 0, 0);

        // wall check
        Gizmos.color = Color.green;
        Gizmos.DrawLine(
            pos,
            pos + forward * _wallCheckDistance
        );

        // ground check
        Gizmos.color = Color.green;

        Vector3 groundCheckPos = pos + new Vector3(dir * 0.4f, 0, 0);

        Gizmos.DrawLine(
            groundCheckPos,
            groundCheckPos + Vector3.down * _groundCheckDistance
        );

        // draw sphere at ground check
        Gizmos.DrawSphere(groundCheckPos, 0.05f);
    }
}