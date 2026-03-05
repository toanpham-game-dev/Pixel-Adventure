using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Builds a grid graph for A* pathfinding using two tilemaps:
/// - groundTilemap
/// - wallTilemap
/// Any cell that has a tile in either tilemap is considered NOT walkable.
/// </summary>
public class GridGraphFromTilemap : MonoBehaviour
{
    // Tilemap containing ground/platform tiles. Presence of a ground tile marks cell as non-walkable.
    [SerializeField] private Tilemap groundTilemap; // Ground / platforms

    // Tilemap containing wall/obstacle tiles. Presence of a wall tile marks cell as non-walkable.
    [SerializeField] private Tilemap wallTilemap;   // Walls / obstacles

    // Size used for gizmo drawing only.
    [SerializeField] private float cellSize = 1f;

    [SerializeField] private bool debugDrawGizmos = true;

    [Header("Agent padding")]
    // How many cells to pad around obstacles so agents keep distance from walls.
    [SerializeField] private int obstaclePadding;

    // Marks cells that originally had a wall tile (used as sources for padding).
    private bool[,] _isWall;

    // Grid nodes and dimensions
    private GridNode[,] _nodes;
    private int _width;
    private int _height;

    // Minimum cell coordinate (xMin, yMin) of the combined tilemap bounds.
    private Vector3Int _origin; // minimum cell coordinates (xMin, yMin)

    private void Awake()
    {
        BuildFromTilemaps();
    }

    // Build the grid covering the union of the two tilemaps.
    // For each cell we compute world position, detect tiles and create GridNode.
    private void BuildFromTilemaps()
    {
        if (groundTilemap == null && wallTilemap == null)
        {
            Debug.LogError("GridGraphFromTilemaps: No tilemaps assigned.");
            return;
        }

        // Use any non-null tilemap for cell<->world conversions.
        Tilemap mainTilemap = groundTilemap != null ? groundTilemap : wallTilemap;

        // Start from main tilemap bounds and expand to include the other tilemap.
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

                // World position = center of this cell
                Vector3 worldPos = mainTilemap.GetCellCenterWorld(cellPos);

                bool hasGround = groundTilemap != null && groundTilemap.HasTile(cellPos);
                bool hasWall = wallTilemap != null && wallTilemap.HasTile(cellPos);

                if (hasWall)
                    _isWall[x, y] = true;

                // Any tile in ground or wall => not walkable
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

    // Return a BoundsInt that covers both inputs (in cell coordinates).
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

    // Convert a world position to the corresponding GridNode (null if outside generated grid).
    public GridNode GetNodeFromWorld(Vector2 worldPos)
    {
        if (_nodes == null) return null;

        // convert world -> cell (using whichever tilemap is available)
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
    /// Expands non-walkable cells by 'obstaclePadding' cells in all directions.
    /// Note: padding is applied around original wall tiles (_isWall). 
    /// The local 'blocked' snapshot records currently non-walkable cells but is not used
    /// for expansion in the current implementation (kept for possible future use).
    /// </summary>
    private void InflateObstacles()
    {
        if (_nodes == null || _isWall == null || obstaclePadding <= 0)
            return;

        int w = _width;
        int h = _height;

        // Snapshot of existing blocked cells (not currently used for expansion).
        bool[,] blocked = new bool[w, h];

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                if (!_nodes[x, y].Walkable)
                    blocked[x, y] = true;
            }
        }

        // Expand obstacles around originally-walled cells so agents keep distance.
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

    // Draw the grid in the editor: green = walkable, red = blocked.
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
