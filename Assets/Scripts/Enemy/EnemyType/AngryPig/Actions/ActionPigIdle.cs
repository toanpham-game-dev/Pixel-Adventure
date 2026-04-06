using UnityEngine;

public class ActionPigIdle : IBehaviorNode
{
    private float _duration;
    private float _timer;
    private bool _started;

    public ActionPigIdle(float duration)
    {
        _duration = duration;
    }

    public NodeState Tick(EnemyContext context, float deltaTime)
    {
        if (context.Status.State != EnemyState.Idle)
        {
            _started = false;
            return NodeState.Failure;
        }

        if (_duration <= 0f)
        {
            context.Status.Move();
            return NodeState.Success;
        }

        if (!_started)
        {
            _timer = _duration;
            _started = true;
        }

        context.Mover?.Stop();

        _timer -= deltaTime;

        if (_timer <= 0f)
        {
            Flip(context);

            context.Status.Move();

            _started = false;

            return NodeState.Success;
        }

        return NodeState.Running;
    }

    private void Flip(EnemyContext context)
    {
        Vector3 scale = context.Self.localScale;
        scale.x *= -1;
        context.Self.localScale = scale;
    }
}