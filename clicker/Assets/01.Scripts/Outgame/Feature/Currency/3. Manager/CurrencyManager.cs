using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    private static CurrencyManager _instance;
    public static CurrencyManager Instance => _instance;

    // DIP
    // 구현체에 의존하지 않고 약속에 의존
    private ICurrencyRepository _repository;

    public static event Action<double> OnDataChanged;

    // 재화 데이터를 배열로 관리
    // 변경에는 닫혀있고, 확장에는 열려있게
    private double[] _currencies = new double[(int)ECurrencyType.Count];

    private void Awake()
    {
        _instance = this;

        _repository = new LocalCurrencyRepository();
        // 무조건 save와 load가 있어야 함 -> 인터페이스
    }

    private void Start()
    {
        double[] currencyValues = _repository.Load().Currencies;
        
        _repository.Load();
    }
    
    // 재화 조회
    public double Get(ECurrencyType type)
    {
        return _currencies[(int)type];
    }

    // 재화 조회 편의 기능
    // 없는 게 클린 코드에 좋지만 있으면 편함
    public double Star => Get(ECurrencyType.Star);

    // 재화 추가
    public void Add(ECurrencyType type, double amount)
    {
        _currencies[(int)type] += amount;

        _repository.Save(new CurrencySaveData()
        {
            Currencies = _currencies
        });

        OnDataChanged?.Invoke(amount);
    }

    // 재화 사용
    public bool TrySpend(ECurrencyType type, double amount)
    {
        if (_currencies[(int)type] >= amount)
        {
            _currencies[(int)type] -= amount;

            _repository.Save(new CurrencySaveData()
            {
                Currencies = _currencies
            });

            OnDataChanged?.Invoke(amount);

            return true;
        }
        return false;
    }

    public bool CanAfford(ECurrencyType type, double amount)
    {
        return _currencies[(int)type] >= amount;
    }
}
