using UnityEngine;

public enum EnemyState
{
    Idle,
    Move,
    Chase,
    Attack,
    Dead,
    FlyUp,
    IsGrounded
}

public abstract class EnemyStatus : MonoBehaviour
{
    public EnemyState State { get; protected set; } = EnemyState.Idle;

    public virtual void Idle()
    {
        State = EnemyState.Idle;
    }

    public virtual void Move()
    {
        State = EnemyState.Move;
    }

    public virtual void Chase()
    {
        State = EnemyState.Chase;
    }

    public virtual void Attack()
    {
        State = EnemyState.Attack;
    }

    public virtual void Dead()
    {
        State = EnemyState.Dead;
    }

    public virtual void FlyUp()
    {
        State = EnemyState.FlyUp;
    }

    public virtual void IsGrounded()
    {
        State = EnemyState.IsGrounded;
    }
}