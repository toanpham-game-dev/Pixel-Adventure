using UnityEngine;

/// <summary>
/// Simple enemy MonoBehaviour that holds and manages an EnemyBrain.
/// On damage, it triggers death which disables the brain and destroys the GameObject after a short delay.
/// </summary>
[RequireComponent(typeof(EnemyBrain))]
public class Enemy : MonoBehaviour
{
    private EnemyBrain _brain;

    private void Awake()
    {
        _brain = GetComponent<EnemyBrain>();
    }

    public void TakeDamage()
    {
        Die();
    }

    private void Die()
    {
        _brain.Enabled = false;
        Destroy(gameObject, 2f);
    }
}
