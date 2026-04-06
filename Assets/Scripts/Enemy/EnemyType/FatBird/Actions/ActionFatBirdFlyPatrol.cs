using UnityEngine;

public class ActionFatBirdFlyPatrol : IBehaviorNode
{
    private float _height;
    private float _speed;
    private Rigidbody2D _rb;

    public ActionFatBirdFlyPatrol(Rigidbody2D rb, float patrolHeight, float patrolSpeed)
    {
        _rb = rb;
        _height = patrolHeight;
        _speed = patrolSpeed;
    }

    public NodeState Tick(EnemyContext context, float deltaTime)
    {
        float targetY = context.Self.position.y + Mathf.Sin(Time.time * _speed) * _height;

        Vector2 pos = _rb.position;
        pos.y = Mathf.MoveTowards(pos.y, targetY, _speed * Time.deltaTime);

        _rb.MovePosition(pos);

        context.Status.Move();

        return NodeState.Running;
    }
}