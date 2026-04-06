using UnityEngine;

public class PlayerHealthPresenter : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private Animator[] _hearts;

    private void Start()
    {
        _playerHealth = FindFirstObjectByType<PlayerHealth>();
        if (_playerHealth != null)
        {
            _playerHealth.HealthChanged += OnHealthChanged;
        }
        UpdateView();
    }

    private void OnDestroy()
    {
        if (_playerHealth != null)
        {
            _playerHealth.HealthChanged -= OnHealthChanged;
        }
    }

    public void Damage(int amount)
    {
        _playerHealth?.DecreaseHealth(amount);
    }

    public void Heal(int amount)
    {
        _playerHealth?.IncreaseHealth(amount);
    }

    public void ResetHealth()
    {
        _playerHealth.RestoreHealth();
    }


    public void UpdateView()
    {
        if (_playerHealth == null) return;

        for (int i = _hearts.Length; i > 0; i--)
        {
            if (i > _playerHealth.CurrentHealth)
            {
                _hearts[i - 1].Play("Hit");
            }
        }
    }

    public void OnHealthChanged()
    {
        UpdateView();
    }
}
