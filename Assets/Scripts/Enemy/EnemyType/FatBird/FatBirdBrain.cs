using UnityEngine;
using System.Collections.Generic;

public class FatBirdBrain : EnemyBrain
{
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private float _detectWidth;
    [SerializeField] private float _detectHeight;
    [SerializeField] private Rigidbody2D _rb;

    [Header("Patrol")]
    [SerializeField] private float _patrolHeight;
    [SerializeField] protected float _patrolSpeed;

    [Header("Attack")]
    [SerializeField] private float _diveSpeed;
    [SerializeField] private float _flyUpSpeed;
    [SerializeField] private Vector2 _originPos;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _groundCheckDistance;

    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody2D>();
        _originPos = transform.position;
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

            // Dive attack
            new SequenceNode(new List<IBehaviorNode>
            {
                new ConditionPlayerBelow(_targetMask, _groundMask, _detectWidth, _detectHeight),
                new ActionDiveAttack(_rb, _diveSpeed, _originPos, _flyUpSpeed, _groundMask, _groundCheckDistance)
            }),

            // Default patrol
            new ActionFatBirdFlyPatrol(_rb, _patrolHeight, _patrolSpeed)
        });
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;

        // ===== PLAYER DETECT BOX =====
        Gizmos.color = Color.green;

        Vector3 boxCenter = pos + Vector3.down * (_detectHeight / 2f);
        Vector3 boxSize = new Vector3(_detectWidth, _detectHeight, 0);

        Gizmos.DrawWireCube(boxCenter, boxSize);

        // ===== GROUND CHECK =====
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(pos, pos + Vector3.down * _groundCheckDistance);

        // ===== ORIGIN POSITION =====
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(_originPos, 0.2f);

        // ===== PATROL RANGE =====
        //Gizmos.color = Color.blue;
        Gizmos.DrawLine(_originPos, _originPos + Vector2.up * _patrolHeight);
        Gizmos.DrawLine(_originPos, _originPos + Vector2.down * _patrolHeight);
    }
}