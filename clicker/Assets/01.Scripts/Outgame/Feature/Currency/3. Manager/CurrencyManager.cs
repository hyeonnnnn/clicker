using System;
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

    public event Action<ECurrencyType, Currency> OnDataChanged;

    private void Awake()
    {
        _instance = this;

        _repository = new LocalCurrencyRepository(AccountManager.Instance.Email);
    }

    private void Start()
    {
        CurrencySaveData saveData = _repository.Load();
        for (int i = 0; i < (int)ECurrencyType.Count; i++)
        {
            _currencies[i] = new Currency(saveData.Currencies[i]);
        }
    }
    
    // 재화 조회
    public Currency Get(ECurrencyType type)
    {
        return _currencies[(int)type];
    }
    public Currency Star => Get(ECurrencyType.Star);

    // 재화 추가
    public void Add(ECurrencyType type, Currency amount)
    {
        _currencies[(int)type] += amount;
        OnDataChanged?.Invoke(type, _currencies[(int)type]);
        Save();
    }

    // 재화 사용
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
}
