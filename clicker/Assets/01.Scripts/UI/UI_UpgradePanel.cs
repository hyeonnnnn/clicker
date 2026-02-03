using System.Collections.Generic;
using UnityEngine;

public class UI_UpgradePanel : MonoBehaviour
{
    [SerializeField] private List<UI_UpgradeItem> _items;

    private void Start()
    {
        Refresh();
        UpgradeManager.OnDataChanged += Refresh;
        CurrencyManager.OnDataChanged += OnCurrencyChanged;
    }

    private void OnDestroy()
    {
        UpgradeManager.OnDataChanged -= Refresh;
        CurrencyManager.OnDataChanged -= OnCurrencyChanged;
    }

    private void OnCurrencyChanged(ECurrencyType type, Currency currency)
    {
        Refresh();
    }

    private void Refresh()
    {
        foreach (var item in _items)
        {
            item.Refresh();
        }
    }
}
