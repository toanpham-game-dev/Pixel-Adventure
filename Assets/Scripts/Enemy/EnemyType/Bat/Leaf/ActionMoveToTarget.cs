using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Behavior Tree action node that makes the enemy (e.g. Bat)
/// continuously chase the target using A* pathfinding.
/// 
/// - Periodically recalculates a path (repath interval)
/// - Follows waypoints using IMover
/// - Always returns Running while chasing
/// - Returns Failure only if no path can be found or dependencies are missing
/// </summary>
public class ActionMoveToTarget : IBehaviorNode
{
    private const float REPATH_INTERVAL = 0.3f;        // How often to recalc A*
    private const float WAYPOINT_REACH_RADIUS = 0.1f;  // Distance to consider a waypoint "reached"

    private float _repathTimer = 0f;
    private List<Vector2> _currentPath;
    private int _currentIndex;

    public NodeState Tick(EnemyContext context, float deltaTime)
    {
        // Ensure required references exist
        if (context.Target == null ||
            context.Pathfinder == null ||
            context.Mover == null)
        {
            return NodeState.Failure;
        }

        Vector2 selfPos = context.Self.position;
        Vector2 targetPos = context.Target.position;

        // Countdown repath timer
        _repathTimer -= deltaTime;

        // Need a new path? (no path yet or repath interval elapsed)
        if (_currentPath == null || _currentPath.Count == 0 || _repathTimer <= 0f)
        {
            _currentPath = context.Pathfinder.FindPath(selfPos, targetPos);
            _currentIndex = 0;
            _repathTimer = REPATH_INTERVAL;

            // If no valid path -> fail and allow Selector/BT to fallback
            if (_currentPath == null || _currentPath.Count == 0)
            {
                context.Mover.Stop();
                return NodeState.Failure;
            }
        }

        // Clamp index just in case
        _currentIndex = Mathf.Clamp(_currentIndex, 0, _currentPath.Count - 1);

        Vector2 waypoint = _currentPath[_currentIndex];

        // If close enough to current waypoint -> advance to next one
        if (Vector2.Distance(selfPos, waypoint) <= WAYPOINT_REACH_RADIUS)
        {
            if (_currentIndex < _currentPath.Count - 1)
            {
                _currentIndex++;
                waypoint = _currentPath[_currentIndex];
            }
            else
            {
                // We are at the last waypoint:
                // Target may have moved, but next repath will handle it.
                // For now, we can move directly toward the target to keep chasing.
                waypoint = targetPos;
            }
        }

        // Move the enemy toward the current waypoint (or directly toward the target)
        context.Mover.MoveTowards(waypoint);

        // Chasing is a continuous behavior -> keep running
        return NodeState.Running;
    }
}
