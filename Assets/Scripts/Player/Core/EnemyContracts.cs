using System;
using UnityEngine;

public interface IHealth
{
    float Current { get; }
    float Max { get; }
    void Damage(float amount);
    event Action<float> OnDamaged;
    event Action OnDied;
}

public interface IEnemyState
{
    void Enter();
    void Update();
    void FixedUpdate();
    void Exit();
}

public interface IMovementStrategy
{
    void Patrol(Rigidbody2D rb, Vector2 dir, EnemyDataSO data);
    void Chase(Rigidbody2D rb, Transform self, Transform target, EnemyDataSO data);
}

public interface IVisionStrategy
{
    bool CanSee(Transform self, Transform target, EnemyDataSO data, LayerMask obstacles);
}

public interface IAttackStrategy
{
    bool CanAttack(Transform self, Transform target, EnemyDataSO data);
    void Attack(Transform self, Transform target, EnemyDataSO data);
    float Cooldown { get; }
}

public interface IEnemyStateRouter
{
    // Resolve state type from requested state type
    StateType Resolve(StateType requested);

    // Create state instance from resolved state type
    EnemyStateMachine CreateResolved(StateType resolved, EnemyController e, EnemyStateMachine sm);

    // Set start state type
    StateType GetStartState();
}
