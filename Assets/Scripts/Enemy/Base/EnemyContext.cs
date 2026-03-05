using UnityEngine;

public class EnemyContext
{
    public Transform Self { get; }
    public Transform Target { get; }
    public IMover Mover { get; }
    public IPathfinder Pathfinder { get; }
    public float AttackRange { get; }
    public float ViewRange { get; }

    public EnemyContext(Transform self, Transform target, IMover mover, IPathfinder pathfinder, float attackRange, float viewRange)
    {
        Self = self;
        Target = target;
        Mover = mover;
        Pathfinder = pathfinder;
        AttackRange = attackRange;
        ViewRange = viewRange;
    }
}
