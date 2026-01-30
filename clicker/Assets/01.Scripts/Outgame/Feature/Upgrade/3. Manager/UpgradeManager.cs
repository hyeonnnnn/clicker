using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [SerializeField] private UpgradeSpecTableSO _specTable;

    private Dictionary<EUpgradeEffect, Upgrade> _upgradeDict = new(); // 실제 업그레이드 상태
    private Dictionary<EUpgradeType, UpgradeGroup> _groups = new(); // 순환 표시 규칙
    private IUpgradeRepository _repository; // 저장, 로드

    public static event Action OnDataChanged;

    private void Awake()
    {
        Instance = this;
        _repository = new JsonUpgradeRepository(); // 저장소 생성
        InitializeUpgrades();
    }

    private void InitializeUpgrades()
    {
        var saveData = _repository.Load();

        foreach (var specData in _specTable.Datas)
        {
            var effects = new List<EUpgradeEffect>();

            foreach (var stepData in specData.Steps)
            {
                if (_upgradeDict.ContainsKey(stepData.Effect))
                {
                    Debug.LogWarning($"업그레이드 이펙트가 중복되었습니다. {stepData.Effect}");
                    continue;
                }

                int savedLevel = 0;
                int effectIndex = (int)stepData.Effect;
                if (saveData.EffectLevels != null && effectIndex < saveData.EffectLevels.Length)
                {
                    savedLevel = saveData.EffectLevels[effectIndex];
                }

                var upgrade = new Upgrade(
                    stepData.Effect, stepData.Description, stepData.MaxLevel,
                    specData.BaseCost, specData.CostMultiplier,
                    stepData.BaseValue, stepData.ValueMultiplier,
                    savedLevel
                );

                // 이펙트 별 업그레이드 상태 채우기
                _upgradeDict[stepData.Effect] = upgrade;

                // 순환할 이펙트 배열 채우기
                effects.Add(stepData.Effect);
            }

            // UpgradeGroup은
            // - 이 타입에서 어떤 이펙트를 돌릴지
            // - 현재 커서가 어디인지
            // - Max면 스킵하는 규칙을 들고 있음

            int savedCursor = 0;
            int typeIndex = (int)specData.Type;
            if (saveData.TypeCursors != null && typeIndex < saveData.TypeCursors.Length)
            {
                savedCursor = saveData.TypeCursors[typeIndex];
            }

            var group = new UpgradeGroup(specData.Type, specData.Name, effects.ToArray(), _upgradeDict, savedCursor);
            _groups[specData.Type] = group;
        }
    }

    // ── 조회 ──
    // UI가 읽을 때 쓰는 것

    public UpgradeGroup GetGroup(EUpgradeType type)
    {
        return _groups.TryGetValue(type, out var group) ? group : null;
    }

    public Upgrade GetUpgrade(EUpgradeEffect effect)
    {
        return _upgradeDict.TryGetValue(effect, out var upgrade) ? upgrade : null;
    }

    // ── 비즈니스 로직 ──

    public bool TryUpgradeType(EUpgradeType type)
    {
        var group = GetGroup(type);
        if (group == null) return false;

        var effect = group.GetCurrentEffect();
        if (effect == null) return false;

        var upgrade = _upgradeDict[effect.Value];
        if (upgrade.IsMaxLevel) return false;

        double cost = upgrade.Cost;
        if (!CurrencyManager.Instance.TrySpend(ECurrencyType.Star, cost))
            return false;

        upgrade.TryLevelUp();
        group.AdvanceToNextAvailable();

        Save();
        OnDataChanged?.Invoke();
        return true;
    }

    public bool CanUpgradeType(EUpgradeType type)
    {
        var group = GetGroup(type);
        if (group == null) return false;

        var effect = group.GetCurrentEffect();
        if (effect == null) return false;

        var upgrade = _upgradeDict[effect.Value];
        if (upgrade.IsMaxLevel) return false;

        return CurrencyManager.Instance.CanAfford(ECurrencyType.Star, upgrade.Cost);
    }

    // ── 저장/불러오기 ──

    private void Save()
    {
        var data = new UpgradeSaveData
        {
            EffectLevels = new int[(int)EUpgradeEffect.Count],
            TypeCursors = new int[(int)EUpgradeType.Count]
        };

        foreach (var pair in _upgradeDict)
        {
            data.EffectLevels[(int)pair.Key] = pair.Value.Level;
        }

        foreach (var pair in _groups)
        {
            data.TypeCursors[(int)pair.Key] = pair.Value.Cursor;
        }

        _repository.Save(data);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) Save();
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}
