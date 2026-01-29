using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [SerializeField] private UpgradeSpecTableSO _specTable;

    private Dictionary<EUpgradeEffect, Upgrade> _upgradeDict = new();

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
            foreach (var stepData in specData.Steps)
            {
                if (_upgradeDict.ContainsKey(stepData.Effect))
                {
                    Debug.LogWarning($"업그레이드 이펙트가 중복되었습니다. {stepData.Effect}");
                    continue;
                }

                var upgrade = new Upgrade(specData, stepData);
                _upgradeDict.Add(stepData.Effect, upgrade);
            }
        }
    }

    public Upgrade Get(EUpgradeEffect effect)
    {
        if (_upgradeDict.TryGetValue(effect, out var upgrade)) return upgrade;
        return null;
    }

    public List<Upgrade> GetAll() => _upgradeDict.Values.ToList();

    public bool TryUpgrade(EUpgradeEffect effect)
    {
        var upgrade = Get(effect);
        if (upgrade == null) return false;
        if (!CanLevelUp(effect)) return false;

        double cost = (double)upgrade.Cost;
        if (!CurrencyManager.Instance.TrySpend(ECurrencyType.Star, cost))
        {
            return false;
        }

        upgrade.TryLevelUp();
        OnDataChanged?.Invoke();
        return true;
    }

    public bool CanLevelUp(EUpgradeEffect effect)
    {
        var upgrade = Get(effect);
        if (upgrade == null) return false;
        if (upgrade.IsMaxLevel) return false;

        double cost = (double)upgrade.Cost;
        return CurrencyManager.Instance.CanAfford(ECurrencyType.Star, cost);
    }
}
