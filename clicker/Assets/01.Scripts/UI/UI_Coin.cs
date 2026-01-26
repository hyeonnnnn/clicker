using TMPro;
using UnityEngine;

public class UI_Coin : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinTextUI;
    [SerializeField] private CoinManager _coinManager;

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
    }
}
