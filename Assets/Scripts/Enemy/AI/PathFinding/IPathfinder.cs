using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Computes a path between two world-space positions.
/// </summary>
public interface IPathfinder
{
    /// <summary>
    /// Finds a path from <paramref name="startWorldPos"/> to <paramref name="targetWorldPos"/>.
    /// </summary>
    List<Vector2> FindPath(Vector2 startWorldPos, Vector2 targetWorldPos);
}
