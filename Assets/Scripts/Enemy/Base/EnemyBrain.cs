using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls enemy AI: creates and ticks a behavior tree each frame,
/// provides a shared EnemyContext and links movement/pathfinding implementations.
/// </summary>
public abstract class EnemyBrain : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] protected Transform target;
    protected IPathfinder Pathfinder;
    [SerializeField] protected EnemyStatus status;

    [Header("AI Stats")]
    [SerializeField] protected float attackRange;
    [SerializeField] protected float viewRange; // Detection

    protected EnemyContext Context;
    protected IMover Mover;
    protected IBehaviorNode RootNode;

    // Enable/disable the brain's ticking externally
    public bool Enabled { get; set; } = true;

    protected virtual void Awake()
    {
        Mover = GetComponent<GridMover2D>();
        status = GetComponent<EnemyStatus>();
    }

    protected virtual void Start()
    {
        target = FindAnyObjectByType<PlayerController>().transform;

        Pathfinder = FindAnyObjectByType<AStarPathfinder>();

        Context = new EnemyContext(
            transform,
            target,
            Mover,
            Pathfinder,
            attackRange,
            viewRange,
            status);

        RootNode = CreateBehaviorTree();
    }

    protected virtual void Update()
    {
        Debug.Log(Context.Status.State);
        // Tick the behavior tree each frame while enabled.
        if (!Enabled || RootNode == null) return;
        RootNode.Tick(Context, Time.deltaTime);
    }

    /// <summary>
    /// Subclasses must construct and return the behavior tree root node.
    /// </summary>
    protected abstract IBehaviorNode CreateBehaviorTree();

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        if (Context == null || Context.DebugPath == null)
            return;

        var path = Context.DebugPath;

        Gizmos.color = Color.yellow;

        for (int i = 0; i < path.Count; i++)
        {
            Gizmos.DrawSphere(path[i], 0.12f);

            if (i < path.Count - 1)
                Gizmos.DrawLine(path[i], path[i + 1]);
        }
    }
}
