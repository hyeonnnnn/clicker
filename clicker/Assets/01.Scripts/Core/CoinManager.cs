using System;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private static CoinManager _instance;
    public static CoinManager Instance => _instance;

    private int _currentCoin = 0;

    public event Action<int> OnCoinChanged;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    public void GetCoin(int amount)
    {
        _currentCoin += amount;
        OnCoinChanged?.Invoke(_currentCoin);
    }
}
