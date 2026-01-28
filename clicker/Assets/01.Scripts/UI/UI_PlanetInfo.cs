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

    [SerializeField] private PlanetPressure _planetPressure;

    private Tween _pressureTween;

    private void Start()
    {
        StageManager.Instance.OnStageChanged += UpdateStageInfo;
        UpdateStageInfo(StageManager.Instance.CurrentStage);

        _planetPressure.OnPressureChanged += UpdatePressureUI;
        UpdatePressureUI(_planetPressure.CurrentPressure, _planetPressure.MaxPressure, true);
    }

    private void OnDestroy()
    {
        StageManager.Instance.OnStageChanged -= UpdateStageInfo;
        _planetPressure.OnPressureChanged -= UpdatePressureUI;
        _pressureTween?.Kill();
    }

    private void UpdateStageInfo(int stageIndex)
    {
        PlanetData data = StageManager.Instance.CurrentPlanetData;

        if (data != null)
        {
            if (_planetNameText != null) _planetNameText.text = data.Name;
            if (_planetIconImage != null) _planetIconImage.sprite = data.Icon;
            if (_planetNumberText != null) _planetNumberText.text = $"{data.Number}";
        }
    }

    private void UpdatePressureUI(double current, double max) => UpdatePressureUI(current, max, false);
    private void UpdatePressureUI(double current, double max, bool immediate)
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

            if (immediate)
            {
                _pressureBar.value = (float)targetValue;
            }
            else
            {
                _pressureTween = _pressureBar.DOValue((float)targetValue, 0.2f)
                    .SetEase(Ease.OutQuad);
            }
        }
    }
}
