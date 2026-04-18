using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Behavior Tree node that makes enemy chase target using A* pathfinding.
/// Includes smoothing and waypoint skipping to avoid zig-zag movement.
/// </summary>
public class ActionMoveToTarget : IBehaviorNode
{
    private const float REPATH_INTERVAL = 0.6f;
    private const float WAYPOINT_REACH_RADIUS = 0.35f;
    private const float TARGET_MOVE_THRESHOLD = 0.5f;

    private float _repathTimer;
    private List<Vector2> _currentPath;
    private int _currentIndex;

    private Vector2 _lastTargetPos;

    public NodeState Tick(EnemyContext context, float deltaTime)
    {
        if (context.Target == null ||
            context.Pathfinder == null ||
            context.Mover == null)
        {
            return NodeState.Failure;
        }

        Vector2 selfPos = context.Self.position;
        Vector2 targetPos = context.Target.position;

        _repathTimer -= deltaTime;

        bool needRepath =
            _currentPath == null ||
            _currentPath.Count == 0 ||
            (_repathTimer <= 0 &&
             Vector2.Distance(targetPos, _lastTargetPos) > TARGET_MOVE_THRESHOLD);

        if (needRepath)
        {
            var newPath = context.Pathfinder.FindPath(selfPos, targetPos);

            if (newPath == null || newPath.Count == 0)
            {
                context.Mover.Stop();
                return NodeState.Failure;
            }

            _currentPath = newPath;

            context.DebugPath = _currentPath;

            if (_currentPath.Count > 1)
                _currentPath.RemoveAt(0);

            _currentIndex = 0;

            _repathTimer = REPATH_INTERVAL;
            _lastTargetPos = targetPos;
        }

        if (_currentIndex >= _currentPath.Count)
        {
            context.Mover.MoveTowards(targetPos);
            return NodeState.Running;
        }

        while (_currentIndex < _currentPath.Count &&
               Vector2.Distance(selfPos, _currentPath[_currentIndex]) <= WAYPOINT_REACH_RADIUS)
        {
            _currentIndex++;
        }

        Vector2 waypoint;

        if (_currentIndex >= _currentPath.Count)
            waypoint = targetPos;
        else
            waypoint = _currentPath[_currentIndex];



        context.Mover.MoveTowards(waypoint);

        return NodeState.Running;
    }
}