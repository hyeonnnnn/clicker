using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using static SoundManager;

public class PlanetPressure : MonoBehaviour
{
    [SerializeField] private ScaleExpansionFeedback _planetExpansion;

    private double _currentPressure;
    private double _maxPressure;

    public double CurrentPressure => _currentPressure;
    public double MaxPressure => _maxPressure;

    public event Action<double, double> OnPressureChanged;
    public event Action OnDepleted;

    private void Awake()
    {
        _planetExpansion = GetComponent<ScaleExpansionFeedback>();
    }

    public void Initialize(double maxPressure, double currentPressure = 0)
    {
        _maxPressure = maxPressure;
        _currentPressure = currentPressure;
        OnPressureChanged?.Invoke(_currentPressure, _maxPressure);
        _planetExpansion.Initialize();
        _planetExpansion.ExpandPlanet(_currentPressure, _maxPressure);
    }

    public void TakeDamage(double damage)
    {
        // 데미지 적용
        _currentPressure += damage;
        _currentPressure = Mathf.Min((float)_currentPressure, (float)_maxPressure);

        // 도메인 객체에 반영
        PlanetManager.Instance.UpdatePressure(_currentPressure);

        // 이벤트 발생
        OnPressureChanged?.Invoke(_currentPressure, _maxPressure);

        CurrencyManager.Instance.Add(ECurrencyType.Star, damage).Forget();
        _planetExpansion.ExpandPlanet(_currentPressure, _maxPressure);

        if (_currentPressure >= _maxPressure)
        {
            OnDepleted?.Invoke();
            SoundManager.Instance.PlaySFX(Sfx.POPPLANET);
        }
    }
}
