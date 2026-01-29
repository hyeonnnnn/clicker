using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [SerializeField] private UpgradeSpecTableSO _specTable;

    private Dictionary<EUpgradeType, Upgrade> _upgradeDict = new();

    public static event Action OnDataChanged;

    private void Awake()
    {
        Instance = this;
        InitializeUpgrades();
    }

    private void InitializeUpgrades()
    {
        foreach (var specData in _specTable.Datas)
        {
            if (_upgradeDict.ContainsKey(specData.Type))
            {
                Debug.LogWarning($"중복된 업그레이드 타입: {specData.Type}");
                continue;
            }

            var upgrade = new Upgrade(specData);
            _upgradeDict.Add(specData.Type, upgrade);
        }
    }

    public Upgrade Get(EUpgradeType type) => _upgradeDict[type] ?? null;

    public Upgrade GetUpgrade(EUpgradeType type)
    {
        if (_upgradeDict.TryGetValue(type, out var upgrade))
        {
            return upgrade;
        }

        Debug.LogError($"업그레이드를 찾을 수 없습니다: {type}");
        return null;
    }

    public bool TryPurchase(EUpgradeType type)
    {
        var upgrade = GetUpgrade(type);
        if (upgrade == null) return false;
        if (upgrade.IsMaxLevel) return false;

        double cost = (double)upgrade.Cost;

        if (!CurrencyManager.Instance.TrySpend(ECurrencyType.Star, cost))
        {
            return false;
        }

        upgrade.TryLevelUp();
        OnDataChanged?.Invoke();
        return true;
    }

    public bool CanAfford(EUpgradeType type)
    {
        var upgrade = GetUpgrade(type);
        if (upgrade == null) return false;
        if (upgrade.IsMaxLevel) return false;

        double cost = (double)upgrade.Cost;
        return CurrencyManager.Instance.CanAfford(ECurrencyType.Star, cost);
    }

    public int GetLevel(EUpgradeType type)
    {
        var upgrade = GetUpgrade(type);
        return upgrade.Level;
    }

    public double GetDamage(EUpgradeType type)
    {
        var upgrade = GetUpgrade(type);
        return upgrade.Damage;
    }

    public Currency GetCost(EUpgradeType type)
    {
        var upgrade = GetUpgrade(type);
        return upgrade.Cost;
    }
}
