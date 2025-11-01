using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObject/EnemyData")]
public class EnemyDataSO : ScriptableObject
{
    [Header("Movement")]
    public float patrolSpeed;
    public float chaseSpeed;

    [Header("Vision")]
    public float viewDistance;

    [Header("Attack")]
    public float attackRange;
    public float attackDamage;
    public float attackCooldown;
}
