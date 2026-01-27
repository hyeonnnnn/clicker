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
    [SerializeField] private Slider _healthBar;

    [SerializeField] private PlanetHealth _planetHealth;

    private Tween _healthTween;

    private void Start()
    {
        StageManager.Instance.OnStageChanged += UpdateStageInfo;
        UpdateStageInfo(StageManager.Instance.CurrentStage);

        _planetHealth.OnHealthChanged += UpdateHealthUI;
        UpdateHealthUI(_planetHealth.CurrentHealth, _planetHealth.MaxHealth, true);
    }

    private void OnDestroy()
    {
        StageManager.Instance.OnStageChanged -= UpdateStageInfo;
        _planetHealth.OnHealthChanged -= UpdateHealthUI;
        _healthTween?.Kill();
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

    private void UpdateHealthUI(int current, int max) => UpdateHealthUI(current, max, false);
    private void UpdateHealthUI(int current, int max, bool immediate)
    {
        if (max <= 0)
        {
            _healthBar.value = 1;
            return;
        }

        float targetValue = (float)current / max;
        _planetHealthText.text = $"{current} / {max}";

        if (_healthBar != null)
        {
            _healthTween?.Kill();

            if (immediate)
            {
                _healthBar.value = targetValue;
            }
            else
            {
                _healthTween = _healthBar.DOValue(targetValue, 0.2f)
                    .SetEase(Ease.OutQuad);
            }
        }
    }
}
