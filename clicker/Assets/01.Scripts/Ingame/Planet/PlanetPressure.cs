using System;
using UnityEngine;
using static SoundManager;

public class PlanetPressure : MonoBehaviour
{
    [SerializeField] private PlanetExpansion _planetExpansion;

    private double _currentPressure;
    private double _maxPressure;

    public double CurrentPressure => _currentPressure;
    public double MaxPressure => _maxPressure;

    public event Action<double, double> OnPressureChanged;
    public event Action OnDepleted;

    private void Awake()
    {
        _planetExpansion = GetComponent<PlanetExpansion>();
    }

    public void Initialize(double maxPressure)
    {
        _maxPressure = maxPressure;
        _currentPressure = 0;
        OnPressureChanged?.Invoke(_currentPressure, _maxPressure);

        _planetExpansion.Initialize();
        _planetExpansion.ExpendPlanet(_currentPressure, _maxPressure);
    }

    public void TakeDamage(double damage)
    {
        _currentPressure += damage;

        _currentPressure = Mathf.Min((float)_currentPressure, (float)_maxPressure);
        OnPressureChanged?.Invoke(_currentPressure, _maxPressure);

        CurrencyManager.Instance.Add(ECurrencyType.Star, damage);
        _planetExpansion.ExpendPlanet(_currentPressure, _maxPressure);

        if (_currentPressure >= _maxPressure)
        {
            OnDepleted?.Invoke();
            SoundManager.Instance.PlaySFX(Sfx.POPPLANET);
        }
    }
}
