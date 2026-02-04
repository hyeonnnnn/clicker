using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    private static CurrencyManager _instance;
    public static CurrencyManager Instance => _instance;

    // DIP
    // 구현체에 의존하지 않고 약속에 의존
    private ICurrencyRepository _repository;

    // 재화 데이터를 배열로 관리
    // 변경에는 닫혀있고, 확장에는 열려있게
    private Currency[] _currencies = new Currency[(int)ECurrencyType.Count];

    public static event Action<ECurrencyType, Currency> OnDataChanged;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        string email = AccountManager.Instance.Email;
        // _repository = new LocalCurrencyRepository(email);
        _repository = new FirebaseCurrencyRepository();
        InitializeCurrency().Forget();
    }

    private async UniTask InitializeCurrency()
    {
        CurrencySaveData saveData = await _repository.Load();
        for (int i = 0; i < (int)ECurrencyType.Count; i++)
        {
            _currencies[i] = new Currency(saveData.Currencies[i]);
        }
    }

    // ── 조회 ──
    public Currency Star => Get(ECurrencyType.Star);
    public Currency Get(ECurrencyType type)
    {
        return _currencies[(int)type];
    }

    // ── 비즈니스 로직 ──
    public async UniTask Add(ECurrencyType type, Currency amount)
    {
        _currencies[(int)type] += amount;
        OnDataChanged?.Invoke(type, _currencies[(int)type]);
        await Save();
    }

    public async UniTask<bool> TrySpend(ECurrencyType type, Currency amount)
    {
        if (_currencies[(int)type] >= amount)
        {
            _currencies[(int)type] -= amount;
            OnDataChanged?.Invoke(type, _currencies[(int)type]);
            await Save();
            return true;
        }
        return false;
    }

    // ── 저장/불러오기 ──

    private async UniTask Save()
    {
        await _repository.Save(new CurrencySaveData()
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
        if (pause) Save().Forget();
    }

    private void OnApplicationQuit()
    {
        Save().Forget();
    }
}
