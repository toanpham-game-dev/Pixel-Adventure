using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls enemy AI: creates and ticks a behavior tree each frame,
/// provides a shared EnemyContext and links movement/pathfinding implementations.
/// </summary>
public abstract class EnemyBrain : MonoBehaviour
{
    // Number of ticks executed within a time window (used for tick rate measurement)
    private int tickCount = 0;

    // Accumulated time to measure ticks per second
    private float tickTimer = 0f;

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
        // Skip update if AI is disabled or no behavior tree is assigned
        if (!Enabled || RootNode == null) return;

        // Start timing for this Behavior Tree tick (performance measurement)
        float startTime = Time.realtimeSinceStartup;

        // Execute one tick of the Behavior Tree
        RootNode.Tick(Context, Time.deltaTime);

        // Calculate execution time (in milliseconds) for this tick
        float tickDuration = (Time.realtimeSinceStartup - startTime) * 1000f;

        // Accumulate tick count for tick rate calculation
        tickCount++;
        tickTimer += Time.deltaTime;

        // Every 1 second -> compute ticks per second
        if (tickTimer >= 1f)
        {
            // Log how many times the Behavior Tree updates per second
            //Debug.Log($"BT Tick Rate: {tickCount} ticks/sec");

            tickCount = 0;
            tickTimer = 0f;
        }

        // Log execution time of each tick (optional, can average later)
        //Debug.Log($"BT Tick Time: {tickDuration:F3} ms");
    }

    /// <summary>
    /// Subclasses must construct and return the behavior tree root node.
    /// </summary>
    protected abstract IBehaviorNode CreateBehaviorTree();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRange);

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
