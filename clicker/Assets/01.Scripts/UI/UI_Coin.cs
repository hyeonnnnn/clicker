using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_Coin : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinTextUI;
    [SerializeField] private CoinManager _coinManager;
    [SerializeField] private float _punchScale = 0.2f;
    [SerializeField] private float _duration = 0.3f;

    private void OnEnable()
    {
        _coinManager.OnCoinChanged += UpdateCoinText;
    }

    private void OnDisable()
    {
        _coinManager.OnCoinChanged -= UpdateCoinText;
    }

    private void UpdateCoinText(int amount)
    {
        _coinTextUI.text = amount.ToString();

        PlayTextEffect();
    }

    private void PlayTextEffect()
    {
        _coinTextUI.transform.DOKill();
        _coinTextUI.transform.localScale = Vector3.one;
        _coinTextUI.transform.DOPunchScale(Vector3.one * _punchScale, _duration);
    }
}
