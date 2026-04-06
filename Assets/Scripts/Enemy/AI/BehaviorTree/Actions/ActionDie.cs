using UnityEngine;

public class ActionDie : IBehaviorNode
{
    private bool _started;
    private float _timer = 1.5f;

    public NodeState Tick(EnemyContext context, float deltaTime)
    {
        if (!_started)
        {
            context.Mover?.Stop();

            context.Status?.Dead();

            _started = true;
        }

        _timer -= deltaTime;

        if (_timer <= 0f)
        {
            GameObject.Destroy(context.Self.gameObject);
            return NodeState.Success;
        }

        return NodeState.Running;
    }
}