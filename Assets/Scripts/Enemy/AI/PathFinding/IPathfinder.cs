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
    /// <param name="startWorldPos">Start position in world space.</param>
    /// <param name="targetWorldPos">Target position in world space.</param>
    /// <returns>Ordered list of world-space waypoints from start to target. Returns an empty list if no path is found.</returns>

    List<Vector2> FindPath(Vector2 startWorldPos, Vector2 targetWorldPos);
}
