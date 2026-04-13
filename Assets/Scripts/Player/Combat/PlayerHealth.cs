using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IPlayerHealth
{
    private const int _minHealth = 0;
    [SerializeField] private int _maxHealth;
    private int _currentHealth;

    public int CurrentHealth
    {
        get => _currentHealth;
        set => _currentHealth = value;
    }
    public int MinHealth => _minHealth;
    public int MaxHealth => _maxHealth;

    public event Action HealthChanged;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void IncreaseHealth(int amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, _minHealth, _maxHealth);
        UpdateHealth();
    }

    public void DecreaseHealth(int amount)
    {
        _currentHealth -= amount;
        _currentHealth = Mathf.Clamp(_currentHealth, _minHealth, _maxHealth);
        UpdateHealth();
    }

    public void RestoreHealth()
    {
        _currentHealth = _maxHealth;
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        HealthChanged?.Invoke();
    }
}