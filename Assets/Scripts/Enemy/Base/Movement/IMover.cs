using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Movement contract for entities that can move directly toward a point,
/// follow an ordered list of waypoints, or stop movement.
/// </summary>
public interface IMover
{
    void MoveDirection(Vector2 direction);
    void MoveTowards(Vector2 target);
    void FollowPath(List<Vector2> path);
    void Stop();
    float MoveSpeed { get; set; }
}
