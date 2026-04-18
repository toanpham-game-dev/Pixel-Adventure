using UnityEngine;

public class ActionIdleTimer : IBehaviorNode
{
    private float _waitTime;
    private float _timer;
    private Rigidbody2D _rb;

    public ActionIdleTimer(float waitTime, Rigidbody2D rb)
    {
        _waitTime = waitTime;
        _rb = rb;
    }

    public NodeState Tick(EnemyContext context, float deltaTime)
    {
        if (context.Status.State != EnemyState.Idle)
            return NodeState.Success;

        _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);

        _timer += deltaTime;

        if (_timer >= _waitTime)
        {
            _timer = 0;
            context.Status.Move();
            return NodeState.Success;
        }

        return NodeState.Running;
    }
}