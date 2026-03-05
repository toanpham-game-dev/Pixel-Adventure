using UnityEngine;

/// <summary>
/// Represents a single cell/node in the pathfinding grid.
/// </summary>
public class GridNode
{
    public Vector2Int GridPos { get; }
    public Vector2 WorldPos { get; }
    public bool Walkable { get; set; }

    public int GCost; // distance from start
    public int HCost; // heuristic to target
    public int FCost => GCost + HCost;

    public GridNode Parent;

    public GridNode(Vector2Int gridPos, Vector2 worldPos, bool walkable)
    {
        GridPos = gridPos;
        WorldPos = worldPos;
        Walkable = walkable;
    }
}
