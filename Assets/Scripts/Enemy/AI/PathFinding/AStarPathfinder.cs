using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A* pathfinding implementation on a tilemap grid.
/// Uses 4-directional movement and Manhattan distance heuristic.
/// </summary>
public class AStarPathfinder : MonoBehaviour, IPathfinder
{
    // Grid graph built from tilemaps
    [SerializeField] private GridGraphFromTilemap grid;

    // Offsets for 4-directional movement
    private static readonly Vector2Int[] Neighbours4 =
    {
        new Vector2Int( 1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int( 0, 1),
        new Vector2Int( 0,-1)
    };

    /// <summary>
    /// Computes a path between two world-space positions.
    /// Returns an empty list if no valid path is found.
    /// </summary>
    public List<Vector2> FindPath(Vector2 startWorldPos, Vector2 targetWorldPos)
    {
        GridNode startNode = grid.GetNodeFromWorld(startWorldPos);
        GridNode targetNode = grid.GetNodeFromWorld(targetWorldPos);

        if (startNode == null || targetNode == null)
            return new List<Vector2>();

        var openSet = new List<GridNode> { startNode };
        var closedSet = new HashSet<GridNode>();

        // Reset previous search data
        ResetNodes();

        startNode.GCost = 0;
        startNode.HCost = GetDistance(startNode, targetNode);

        // A* search loop
        while (openSet.Count > 0)
        {
            GridNode current = GetLowestFCost(openSet);

            if (current == targetNode)
                return RetracePath(startNode, targetNode);

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (var neighbour in GetNeighbours(current))
            {
                if (!neighbour.Walkable || closedSet.Contains(neighbour))
                    continue;

                int newGCost = current.GCost + GetDistance(current, neighbour);

                // Update node if a shorter path is found
                if (newGCost < neighbour.GCost || !openSet.Contains(neighbour))
                {
                    neighbour.GCost = newGCost;
                    neighbour.HCost = GetDistance(neighbour, targetNode);
                    neighbour.Parent = current;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        return new List<Vector2>();
    }

    /// <summary>
    /// Clears pathfinding costs and parent references for all nodes.
    /// </summary>
    private void ResetNodes()
    {
        var nodes = grid.Nodes;
        int w = grid.Width;
        int h = grid.Height;

        for (int x = 0; x < w; x++)
            for (int y = 0; y < h; y++)
            {
                nodes[x, y].GCost = int.MaxValue;
                nodes[x, y].HCost = 0;
                nodes[x, y].Parent = null;
            }
    }

    /// <summary>
    /// Returns the node with the lowest F cost in the list.
    /// If tied, prefers the node with lower H cost.
    /// </summary>
    private GridNode GetLowestFCost(List<GridNode> list)
    {
        GridNode lowest = list[0];

        for (int i = 1; i < list.Count; i++)
        {
            if (list[i].FCost < lowest.FCost ||
                (list[i].FCost == lowest.FCost && list[i].HCost < lowest.HCost))
            {
                lowest = list[i];
            }
        }

        return lowest;
    }

    /// <summary>
    /// Returns the valid neighbouring nodes (4-directional).
    /// </summary>
    private IEnumerable<GridNode> GetNeighbours(GridNode node)
    {
        foreach (var offset in Neighbours4)
        {
            GridNode n = grid.GetNode(node.GridPos.x + offset.x, node.GridPos.y + offset.y);
            if (n != null)
                yield return n;
        }
    }

    /// <summary>
    /// Manhattan distance heuristic.
    /// </summary>
    private int GetDistance(GridNode a, GridNode b)
    {
        int dx = Mathf.Abs(a.GridPos.x - b.GridPos.x);
        int dy = Mathf.Abs(a.GridPos.y - b.GridPos.y);
        return dx + dy;
    }

    /// <summary>
    /// Reconstructs the path by following parent links.
    /// </summary>
    private List<Vector2> RetracePath(GridNode startNode, GridNode endNode)
    {
        var path = new List<Vector2>();
        GridNode current = endNode;

        while (current != startNode)
        {
            path.Add(current.WorldPos);
            current = current.Parent;
        }

        path.Reverse();
        return path;
    }
}