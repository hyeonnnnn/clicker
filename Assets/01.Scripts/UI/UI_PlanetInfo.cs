using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlanetInfo : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image _planetIconImage;
    [SerializeField] private TextMeshProUGUI _planetNumberText;
    [SerializeField] private TextMeshProUGUI _planetNameText;
    [SerializeField] private TextMeshProUGUI _planetHealthText;
    [SerializeField] private Slider _pressureBar;

    private Tween _pressureTween;

    private void OnEnable()
    {
        PlanetManager.OnDataChanged += UpdateStageInfo; // 정보창 업데이트
        PlanetManager.OnPressureChanged += UpdatePressureUI; // 슬라이더 업데이트

        if (PlanetManager.Instance != null && PlanetManager.Instance.CurrentPlanet != null)
        {
            UpdateStageInfo();
        }
    }

    private void OnDisable()
    {
        PlanetManager.OnDataChanged -= UpdateStageInfo;
        PlanetManager.OnPressureChanged -= UpdatePressureUI;
        _pressureTween?.Kill();
    }

    private void UpdateStageInfo()
    {
        // 정보창 초기화
        var data = StageController.Instance.GetPlanetInfoViewData();

        if (_planetNameText != null) _planetNameText.text = data.Name;
        if (_planetIconImage != null) _planetIconImage.sprite = data.Icon;
        if (_planetNumberText != null) _planetNumberText.text = data.Level;

        // 슬라이더 초기화
        var planet = PlanetManager.Instance.CurrentPlanet;
        UpdatePressureUI(planet.CurrentPressure, planet.MaxPressure);
    }

    private void UpdatePressureUI(double current, double max)
    {
        if (max <= 0)
        {
            _pressureBar.value = 0;
            return;
        }

        double targetValue = current / max;
        _planetHealthText.text = $"{current.ToFormattedString()} / {max.ToFormattedString()}";

        if (_pressureBar != null)
        {
            _pressureTween?.Kill();
            _pressureTween = _pressureBar.DOValue((float)targetValue, 0.2f)
                                .SetEase(Ease.OutQuad);
        }

    }
}
