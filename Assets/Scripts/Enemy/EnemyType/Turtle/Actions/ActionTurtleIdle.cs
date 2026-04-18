using UnityEngine;

public class ActionTurtleIdle : IBehaviorNode
{
    private float _idleTime;
    private float _spikeTime;

    private float _timer;
    private bool _isSpike;

    public ActionTurtleIdle(float idleTime, float spikeTime)
    {
        _idleTime = idleTime;
        _spikeTime = spikeTime;
    }

    public NodeState Tick(EnemyContext context, float deltaTime)
    {
        _timer += Time.deltaTime;

        if (!_isSpike)
        {
            context.Status.Idle();

            if (_timer >= _idleTime)
            {
                _timer = 0;
                _isSpike = true;
            }
        }
        else
        {
            context.Status.Attack();

            if (_timer >= _spikeTime)
            {
                _timer = 0;
                _isSpike = false;
            }
        }

        return NodeState.Running;
    }
}