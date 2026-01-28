using System;
using UnityEngine;
using static SoundManager;

public class PlanetPressure : MonoBehaviour
{
    [SerializeField] private PlanetExpansion _planetExpansion;

    private int _currentPressure;
    private int _maxPressure;

    public int CurrentPressure => _currentPressure;
    public int MaxPressure => _maxPressure;

    public event Action<int, int> OnPressureChanged;
    public event Action OnDepleted;

    private void Awake()
    {
        _planetExpansion = GetComponent<PlanetExpansion>();
    }

    public void Initialize(int maxPressure)
    {
        _maxPressure = maxPressure;
        _currentPressure = 0;
        OnPressureChanged?.Invoke(_currentPressure, _maxPressure);

        _planetExpansion.Initialize();
        _planetExpansion.ExpendPlanet(_currentPressure, _maxPressure);
    }

    public void TakeDamage(int damage)
    {
        _currentPressure += damage;

        _currentPressure = Mathf.Min(_currentPressure, _maxPressure);
        OnPressureChanged?.Invoke(_currentPressure, _maxPressure);

        CoinManager.Instance.GetCoin(damage);
        _planetExpansion.ExpendPlanet(_currentPressure, _maxPressure);

        _planetExpansion.ExpendPlanet(_currentPressure, _maxPressure);

        if (_currentPressure >= _maxPressure)
        {
            OnDepleted?.Invoke();
            SoundManager.Instance.PlaySFX(Sfx.POPPLANET);
        }
    }
}
