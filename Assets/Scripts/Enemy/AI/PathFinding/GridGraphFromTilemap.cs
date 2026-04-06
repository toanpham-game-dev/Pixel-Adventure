using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Builds a grid graph from tilemaps for A* pathfinding.
/// Any tile in the ground or wall tilemap is treated as non-walkable.
/// </summary>
public class GridGraphFromTilemap : MonoBehaviour
{
    // Tilemap containing ground/platform tiles
    [SerializeField] private Tilemap groundTilemap;

    // Tilemap containing walls or obstacles
    [SerializeField] private Tilemap wallTilemap;

    // Size used only for gizmo visualization
    [SerializeField] private float cellSize = 1f;

    [SerializeField] private bool debugDrawGizmos = true;

    [Header("Agent padding")]
    // Extra blocked cells around walls so agents keep distance
    [SerializeField] private int obstaclePadding;

    // Marks cells that originally contained wall tiles
    private bool[,] _isWall;

    private GridNode[,] _nodes;
    private int _width;
    private int _height;

    // Minimum grid cell coordinate of the combined tilemap bounds
    private Vector3Int _origin;

    private void Awake()
    {
        BuildFromTilemaps();
    }

    /// <summary>
    /// Generates the grid covering the union of both tilemaps.
    /// </summary>
    private void BuildFromTilemaps()
    {
        if (groundTilemap == null && wallTilemap == null)
        {
            Debug.LogError("GridGraphFromTilemaps: No tilemaps assigned.");
            return;
        }

        Tilemap mainTilemap = groundTilemap != null ? groundTilemap : wallTilemap;

        BoundsInt bounds = mainTilemap.cellBounds;

        if (groundTilemap != null)
            bounds = Encapsulate(bounds, groundTilemap.cellBounds);

        if (wallTilemap != null)
            bounds = Encapsulate(bounds, wallTilemap.cellBounds);

        _origin = new Vector3Int(bounds.xMin, bounds.yMin, 0);
        _width = bounds.size.x;
        _height = bounds.size.y;

        _nodes = new GridNode[_width, _height];
        _isWall = new bool[_width, _height];

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Vector3Int cellPos = new Vector3Int(_origin.x + x, _origin.y + y, 0);

                // Center position of this cell in world space
                Vector3 worldPos = mainTilemap.GetCellCenterWorld(cellPos);

                bool hasGround = groundTilemap != null && groundTilemap.HasTile(cellPos);
                bool hasWall = wallTilemap != null && wallTilemap.HasTile(cellPos);

                if (hasWall)
                    _isWall[x, y] = true;

                // Any tile present means the cell is blocked
                bool walkable = !(hasGround || hasWall);

                _nodes[x, y] = new GridNode(
                    new Vector2Int(x, y),
                    worldPos,
                    walkable
                );
            }
        }

        InflateObstacles();
    }

    /// <summary>
    /// Returns bounds that cover both input bounds.
    /// </summary>
    private BoundsInt Encapsulate(BoundsInt a, BoundsInt b)
    {
        int xMin = Mathf.Min(a.xMin, b.xMin);
        int yMin = Mathf.Min(a.yMin, b.yMin);
        int xMax = Mathf.Max(a.xMax, b.xMax);
        int yMax = Mathf.Max(a.yMax, b.yMax);

        return new BoundsInt(
            xMin, yMin, 0,
            xMax - xMin, yMax - yMin, 1
        );
    }

    /// <summary>
    /// Converts a world-space position to the corresponding grid node.
    /// </summary>
    public GridNode GetNodeFromWorld(Vector2 worldPos)
    {
        if (_nodes == null) return null;

        Tilemap mainTilemap = groundTilemap != null ? groundTilemap : wallTilemap;
        Vector3Int cell = mainTilemap.WorldToCell(worldPos);

        int x = cell.x - _origin.x;
        int y = cell.y - _origin.y;

        if (x < 0 || x >= _width || y < 0 || y >= _height)
            return null;

        return _nodes[x, y];
    }

    public GridNode GetNode(int x, int y)
    {
        if (x < 0 || x >= _width || y < 0 || y >= _height) return null;
        return _nodes[x, y];
    }

    public GridNode[,] Nodes => _nodes;
    public int Width => _width;
    public int Height => _height;

    /// <summary>
    /// Expands obstacles by obstaclePadding cells to keep agents away from walls.
    /// </summary>
    private void InflateObstacles()
    {
        if (_nodes == null || _isWall == null || obstaclePadding <= 0)
            return;

        int w = _width;
        int h = _height;

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                if (!_isWall[x, y]) continue;

                for (int ox = -obstaclePadding; ox <= obstaclePadding; ox++)
                {
                    for (int oy = -obstaclePadding; oy <= obstaclePadding; oy++)
                    {
                        int nx = x + ox;
                        int ny = y + oy;

                        if (nx < 0 || nx >= w || ny < 0 || ny >= h)
                            continue;

                        _nodes[nx, ny].Walkable = false;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Draws the grid in the editor for debugging.
    /// Green = walkable, Red = blocked.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (!debugDrawGizmos || _nodes == null) return;

        foreach (var node in _nodes)
        {
            Gizmos.color = node.Walkable ? Color.green : Color.red;
            Gizmos.DrawWireCube(node.WorldPos, Vector3.one * (cellSize * 0.9f));
        }
    }
}