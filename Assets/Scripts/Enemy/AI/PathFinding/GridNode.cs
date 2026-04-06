using UnityEngine;

/// <summary>
/// Represents a single node in the pathfinding grid.
/// Stores position data and A* pathfinding costs.
/// </summary>
public class GridNode
{
    // Grid coordinate inside the graph
    public Vector2Int GridPos { get; }

    // World-space position used for movement
    public Vector2 WorldPos { get; }

    // Whether the node can be traversed
    public bool Walkable { get; set; }

    // Cost from start node
    public int GCost;

    // Heuristic cost to target node
    public int HCost;

    // Total cost used by A* (F = G + H)
    public int FCost => GCost + HCost;

    // Previous node in the computed path
    public GridNode Parent;

    public GridNode(Vector2Int gridPos, Vector2 worldPos, bool walkable)
    {
        GridPos = gridPos;
        WorldPos = worldPos;
        Walkable = walkable;
    }
}