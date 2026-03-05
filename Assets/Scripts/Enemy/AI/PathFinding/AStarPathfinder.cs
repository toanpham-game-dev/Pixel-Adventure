using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A* pathfinding implementation on a tilemap-based grid.
/// Uses 4-directional movement (no diagonals) and Manhattan distance as heuristic.
/// </summary>
public class AStarPathfinder : MonoBehaviour, IPathfinder
{
    /// <summary>
    /// Reference to the grid graph generated from a Tilemap.
    /// Provides access to nodes, dimensions, and world/grid conversions.
    /// </summary>
    [SerializeField] private GridGraphFromTilemap grid;

    /// <summary>
    /// 4-directional neighbor offsets (right, left, up, down).
    /// </summary>
    private static readonly Vector2Int[] Neighbours4 =
    {
        new Vector2Int( 1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int( 0, 1),
        new Vector2Int( 0,-1)
    };

    /// <summary>
    /// Finds a path between two world positions using A*.
    /// </summary>
    /// <param name="startWorldPos">Start position in world space.</param>
    /// <param name="targetWorldPos">Target position in world space.</param>
    /// <returns>
    /// A list of world positions forming the path (start excluded, target included).
    /// Returns an empty list if no path is found or positions are invalid.
    /// </returns>
    public List<Vector2> FindPath(Vector2 startWorldPos, Vector2 targetWorldPos)
    {
        // Convert world positions to grid nodes
        GridNode startNode = grid.GetNodeFromWorld(startWorldPos);
        GridNode targetNode = grid.GetNodeFromWorld(targetWorldPos);

        // If either position is outside the grid, no path can be computed
        if (startNode == null || targetNode == null)
            return new List<Vector2>();

        // Nodes to be evaluated (open set) and already evaluated (closed set)
        var openSet = new List<GridNode> { startNode };
        var closedSet = new HashSet<GridNode>();

        // Clear previous search data stored on nodes
        ResetNodes();

        // Initialize start costs
        startNode.GCost = 0;
        startNode.HCost = GetDistance(startNode, targetNode);

        // Main A* loop
        while (openSet.Count > 0)
        {
            // Pick the node with the lowest F cost (G + H)
            GridNode current = GetLowestFCost(openSet);

            // Target reached: reconstruct the path
            if (current == targetNode)
                return RetracePath(startNode, targetNode);

            openSet.Remove(current);
            closedSet.Add(current);

            // Evaluate neighbors
            foreach (var neighbour in GetNeighbours(current))
            {
                // Skip blocked nodes or nodes already evaluated
                if (!neighbour.Walkable || closedSet.Contains(neighbour))
                    continue;

                // Tentative cost from start to this neighbor via current
                int newGCost = current.GCost + GetDistance(current, neighbour);

                // Update if this route is better, or neighbour has not been discovered yet
                if (newGCost < neighbour.GCost || !openSet.Contains(neighbour))
                {
                    neighbour.GCost = newGCost;
                    neighbour.HCost = GetDistance(neighbour, targetNode);
                    neighbour.Parent = current;

                    // Add to open set if not already present
                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        // No valid path found
        return new List<Vector2>();
    }

    /// <summary>
    /// Resets pathfinding values on all nodes (G/H costs and parent links).
    /// Must be called before running a new search.
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
    /// Returns the node with the lowest F cost from the given list.
    /// If there is a tie, the node with the lower H cost is preferred.
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
    /// Enumerates valid neighbors of the given node using 4-directional movement.
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
    /// Manhattan distance heuristic (dx + dy).
    /// Suitable for 4-directional grid movement.
    /// </summary>
    private int GetDistance(GridNode a, GridNode b)
    {
        int dx = Mathf.Abs(a.GridPos.x - b.GridPos.x);
        int dy = Mathf.Abs(a.GridPos.y - b.GridPos.y);
        return dx + dy;
    }

    /// <summary>
    /// Reconstructs the path by walking backward from endNode to startNode
    /// using Parent links, then reversing the list.
    /// </summary>
    private List<Vector2> RetracePath(GridNode startNode, GridNode endNode)
    {
        var path = new List<Vector2>();
        GridNode current = endNode;

        // Follow parent links from target back to start
        while (current != startNode)
        {
            path.Add(current.WorldPos);
            current = current.Parent;
        }

        // Reverse to get start -> target order
        path.Reverse();
        return path;
    }
}
