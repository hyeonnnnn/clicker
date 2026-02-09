using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [SerializeField] private UpgradeSpecTableSO _specTable;
    private HybridRepository<UpgradeSaveData> _repository;

    private Dictionary<EUpgradeEffect, Upgrade> _upgradeDict = new(); // 실제 업그레이드 상태
    private Dictionary<EUpgradeType, UpgradeGroup> _groupDict = new(); // 순환 표시 규칙
    
    public bool IsInitialized { get; private set; }

    public static event Action OnDataChanged;
    public static event Action<EUpgradeEffect> OnUpgraded;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        string userId = AccountManager.Instance.Email;
        _repository = new HybridRepository<UpgradeSaveData>(
            new JsonUpgradeRepository(userId),
#if !UNITY_WEBGL || UNITY_EDITOR
            new FirebaseUpgradeRepository()
#else
            null
#endif
        );
        InitializeUpgrades().Forget();
    }

    private async UniTask InitializeUpgrades()
    {
        var saveData = await _repository.Load();

        foreach (var specData in _specTable.Datas)
        {
            var effects = CreateUpgrades(specData, saveData);
            CreateGroup(specData, effects, saveData);
        }

        IsInitialized = true;
    }

    private List<EUpgradeEffect> CreateUpgrades(UpgradeSpecData specData, UpgradeSaveData saveData)
    {
        var effects = new List<EUpgradeEffect>();

        foreach (var stepData in specData.Steps)
        {
            if (_upgradeDict.ContainsKey(stepData.Effect))
            {
                Debug.LogWarning($"업그레이드 이펙트가 중복되었습니다. {stepData.Effect}");
                continue;
            }

            int savedLevel = GetSavedLevel(saveData, stepData.Effect);
            var upgrade = new Upgrade(stepData, specData, savedLevel);

            _upgradeDict[stepData.Effect] = upgrade;
            effects.Add(stepData.Effect);
        }

        return effects;
    }

    private void CreateGroup(UpgradeSpecData specData, List<EUpgradeEffect> effects, UpgradeSaveData saveData)
    {
        int savedCursor = GetSavedCursor(saveData, specData.Type);
        var group = new UpgradeGroup(specData.Type, specData.Name, effects.ToArray(), _upgradeDict, savedCursor);
        _groupDict[specData.Type] = group;
    }

    private int GetSavedLevel(UpgradeSaveData saveData, EUpgradeEffect effect)
    {
        int index = (int)effect;
        if (saveData.EffectLevels != null && index < saveData.EffectLevels.Length)
            return saveData.EffectLevels[index];
        return 0;
    }

    private int GetSavedCursor(UpgradeSaveData saveData, EUpgradeType type)
    {
        int index = (int)type;
        if (saveData.TypeCursors != null && index < saveData.TypeCursors.Length)
            return saveData.TypeCursors[index];
        return 0;
    }

    // ── 조회 ──
    // UI가 읽을 때 쓰는 것
    public UpgradeGroup GetGroup(EUpgradeType type)
    {
        return _groupDict.TryGetValue(type, out var group) ? group : null;
    }

    public Upgrade GetUpgrade(EUpgradeEffect effect)
    {
        return _upgradeDict.TryGetValue(effect, out var upgrade) ? upgrade : null;
    }

    // ── 비즈니스 로직 ──

    public bool TryLevelUp(EUpgradeType type)
    {
        var group = GetGroup(type);
        if (group == null) return false;

        var effect = group.GetCurrentEffect();
        if (effect == null) return false;

        var upgrade = _upgradeDict[effect.Value];

        double cost = upgrade.Cost;
        if (!CurrencyManager.Instance.TrySpend(ECurrencyType.Star, cost))
            return false;

        upgrade.TryLevelUp();
        group.AdvanceToNextAvailable();

        Save();
        OnDataChanged?.Invoke();
        OnUpgraded?.Invoke(effect.Value);
        return true;
    }

    public bool CanLevelUp(EUpgradeType type)
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

        foreach (var pair in _groupDict)
        {
            data.TypeCursors[(int)pair.Key] = pair.Value.Cursor;
        }

        _repository.Save(data);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Save();
            _repository.ForceRemoteSave().Forget();
        }
    }

    private void OnApplicationQuit()
    {
        Save();
        _repository.ForceRemoteSave().Forget();
    }
}
