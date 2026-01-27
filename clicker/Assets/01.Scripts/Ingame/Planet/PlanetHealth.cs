using System;
using UnityEngine;

public class PlanetHealth : MonoBehaviour
{
    private int _currentHealth;
    private int _maxHealth;

    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _maxHealth;

    public event Action<int, int> OnHealthChanged;
    public event Action OnDepleted;

    public void Initialize(int maxHealth)
    {
        _maxHealth = maxHealth;
        _currentHealth = maxHealth;
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Max(0, _currentHealth);
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

        CoinManager.Instance.GetCoin(damage);

        if (_currentHealth <= 0)
        {
            OnDepleted?.Invoke();
        }
    }
}
