using UnityEngine;

/// <summary>
/// Controls enemy AI: creates and ticks a behavior tree each frame,
/// provides a shared EnemyContext and links movement/pathfinding implementations.
/// </summary>
public abstract class EnemyBrain : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] protected Transform target;
    [SerializeField] protected MonoBehaviour pathfinderMono;
    protected IPathfinder Pathfinder;

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
        Pathfinder = pathfinderMono as IPathfinder;
    }

    protected virtual void Start()
    {
        // Build the shared context and create the root behavior node.
        Context = new EnemyContext(
            transform,
            target,
            Mover,
            Pathfinder,
            attackRange,
            viewRange);

        RootNode = CreateBehaviorTree();
    }

    protected virtual void Update()
    {
        // Tick the behavior tree each frame while enabled.
        if (!Enabled || RootNode == null) return;
        RootNode.Tick(Context, Time.deltaTime);
    }

    /// <summary>
    /// Subclasses must construct and return the behavior tree root node.
    /// </summary>
    protected abstract IBehaviorNode CreateBehaviorTree();
}
