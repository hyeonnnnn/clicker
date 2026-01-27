using System;
using UnityEngine;

public class PlanetPressure : MonoBehaviour
{
    private int _currentPressure;
    private int _maxPressure;

    public int CurrentPressure => _currentPressure;
    public int MaxPressure => _maxPressure;

    public event Action<int, int> OnPressureChanged;
    public event Action OnDepleted;

    public void Initialize(int maxHealth)
    {
        _maxPressure = maxHealth;
        _currentPressure = 0;
        OnPressureChanged?.Invoke(_currentPressure, _maxPressure);
    }

    public void TakeDamage(int damage)
    {
        _currentPressure += damage;
        _currentPressure = Mathf.Min(_maxPressure, _currentPressure);
        OnPressureChanged?.Invoke(_currentPressure, _maxPressure);

        CoinManager.Instance.GetCoin(damage);

        if (_currentPressure >= _maxPressure)
        {
            OnDepleted?.Invoke();
        }
    }
}
