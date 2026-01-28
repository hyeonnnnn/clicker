using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Star : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinTextUI;
    [SerializeField] private Image _coinImageUI;
    [SerializeField] private CurrencyManager _currencyManager;

    [Header("Text Effect")]
    [SerializeField] private float _punchScale = 0.2f;
    [SerializeField] private float _scaleDuration = 0.3f;

    [Header("Icon Effect")]
    [SerializeField] private float _shakeStrength = 10f;
    [SerializeField] private float _shakeDuration = 0.3f;

    private void OnEnable()
    {
        CurrencyManager.OnDataChanged += UpdateStarText;
    }

    private void OnDisable()
    {
        CurrencyManager.OnDataChanged -= UpdateStarText;
    }

    private void UpdateStarText(double amount)
    {
        Currency star = CurrencyManager.Instance.Star;
        _coinTextUI.text = $"{star}";

        PlayScaleEffect(_coinTextUI.transform);
        PlayShakeEffect(_coinImageUI.transform);
    }

    private void PlayScaleEffect(Transform target)
    {
        target.DOKill();
        target.localScale = Vector3.one;
        target.DOPunchScale(Vector3.one * _punchScale, _scaleDuration);
    }

    private void PlayShakeEffect(Transform target)
    {
        target.DOKill();
        target.localRotation = Quaternion.identity;
        target.DOPunchRotation(new Vector3(0f, 0f, _shakeStrength), _shakeDuration);
    }
}
