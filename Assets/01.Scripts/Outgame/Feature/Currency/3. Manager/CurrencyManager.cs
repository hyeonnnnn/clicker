using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    private HybridRepository<CurrencySaveData> _repository;

    // 재화 데이터를 배열로 관리
    // 변경에는 닫혀있고, 확장에는 열려있게
    private Currency[] _currencies = new Currency[(int)ECurrencyType.Count];

    public static event Action<ECurrencyType, Currency> OnDataChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _repository = new HybridRepository<CurrencySaveData>(
            new LocalCurrencyRepository(AccountManager.Instance.Email),
#if !UNITY_WEBGL || UNITY_EDITOR
            new FirebaseCurrencyRepository()
#else
            null
#endif
        );
        InitializeCurrency().Forget();
    }

    private async UniTask InitializeCurrency()
    {
        CurrencySaveData saveData = await _repository.Load();
        for (int i = 0; i < (int)ECurrencyType.Count; i++)
        {
            _currencies[i] = new Currency(saveData.Currencies[i]);
            OnDataChanged?.Invoke((ECurrencyType)i, _currencies[i]);
        }
    }

    // ── 조회 ──
    public Currency Star => Get(ECurrencyType.Star);
    public Currency Get(ECurrencyType type)
    {
        return _currencies[(int)type];
    }

    // ── 비즈니스 로직 ──
    public void Add(ECurrencyType type, Currency amount)
    {
        _currencies[(int)type] += amount;
        OnDataChanged?.Invoke(type, _currencies[(int)type]);
        Save();
    }

    public bool TrySpend(ECurrencyType type, Currency amount)
    {
        if (_currencies[(int)type] >= amount)
        {
            _currencies[(int)type] -= amount;
            OnDataChanged?.Invoke(type, _currencies[(int)type]);
            Save();
            return true;
        }
        return false;
    }

    // ── 저장/불러오기 ──

    private void Save()
    {
        _repository.Save(new CurrencySaveData()
        {
            Currencies = ToSaveData()
        });
    }

    private double[] ToSaveData()
    {
        double[] result = new double[_currencies.Length];
        for (int i = 0; i < _currencies.Length; i++)
        {
            result[i] = (double)_currencies[i];
        }
        return result;
    }

    public bool CanAfford(ECurrencyType type, Currency amount)
    {
        return _currencies[(int)type] >= amount;
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
