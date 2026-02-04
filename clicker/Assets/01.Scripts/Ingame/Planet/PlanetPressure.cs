using System;
using UnityEngine;

public class PlanetPressure : MonoBehaviour
{
    private ScaleExpansionFeedback _planetExpansion;

    private double _currentPressure;
    private double _maxPressure;

    public double CurrentPressure => _currentPressure;
    public double MaxPressure => _maxPressure;

    public event Action<double> OnDamaged;
    public event Action OnDepleted;

    private void Awake()
    {
        _planetExpansion = GetComponent<ScaleExpansionFeedback>();
    }

    private void Start()
    {
        PlanetManager.OnDataChanged += OnPlanetDataChanged;

        if (PlanetManager.Instance.CurrentPlanet != null)
        {
            OnPlanetDataChanged();
        }
    }

    private void OnDestroy()
    {
        PlanetManager.OnDataChanged -= OnPlanetDataChanged;
    }

    private void OnPlanetDataChanged()
    {
        var planet = PlanetManager.Instance.CurrentPlanet;
        Initialize(planet.MaxPressure, planet.CurrentPressure);
    }

    public void Initialize(double maxPressure, double currentPressure = 0)
    {
        _maxPressure = maxPressure;
        _currentPressure = currentPressure;

        _planetExpansion.Initialize();
        _planetExpansion.ExpandPlanet(_currentPressure, _maxPressure);
    }

    public void TakeDamage(double damage)
    {
        if (_currentPressure >= _maxPressure) return;

        _currentPressure = Math.Min(_currentPressure + damage, _maxPressure);

        PlanetManager.Instance.UpdatePressure(_currentPressure);

        OnDamaged?.Invoke(damage);
        _planetExpansion.ExpandPlanet(_currentPressure, _maxPressure);

        if (_currentPressure >= _maxPressure)
        {
            OnDepleted?.Invoke();
            PlanetManager.Instance.NextStage();
        }
    }
}
