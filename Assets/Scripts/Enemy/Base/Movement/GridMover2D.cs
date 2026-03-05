using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Moves a Rigidbody2D along world-space waypoints or directly toward a target.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class GridMover2D : MonoBehaviour, IMover
{
    // Movement speed (units per second)
    [SerializeField] private float moveSpeed;

    // Cached Rigidbody2D for physics-based movement
    private Rigidbody2D _rb;

    // Current path as world-space waypoints
    private List<Vector2> _currentPath;

    // Index of the current waypoint in the path
    private int _currentIndex;

    private void Awake()
    {
        // Cache the Rigidbody2D reference
        _rb = GetComponent<Rigidbody2D>();
    }

    // Move directly towards a single target position
    public void MoveTowards(Vector2 target)
    {
        Vector2 dir = (target - _rb.position).normalized;
        _rb.linearVelocity = dir * moveSpeed;
    }

    // Follow an ordered list of world-space waypoints
    public void FollowPath(List<Vector2> path)
    {
        if (path == null || path.Count == 0)
        {
            Stop();
            return;
        }

        _currentPath = path;

        if (_currentIndex >= _currentPath.Count)
        {
            Stop();
            return;
        }

        Vector2 waypoint = _currentPath[_currentIndex];

        // Advance to next waypoint when close enough
        if (Vector2.Distance(_rb.position, waypoint) < 0.1f)
        {
            _currentIndex++;
            if (_currentIndex >= _currentPath.Count)
            {
                Stop();
                return;
            }

            waypoint = _currentPath[_currentIndex];
        }

        Vector2 dir = (waypoint - _rb.position).normalized;
        _rb.linearVelocity = dir * moveSpeed;
    }

    // Stop movement and clear current path
    public void Stop()
    {
        _rb.linearVelocity = Vector2.zero;
        _currentPath = null;
        _currentIndex = 0;
    }
}
