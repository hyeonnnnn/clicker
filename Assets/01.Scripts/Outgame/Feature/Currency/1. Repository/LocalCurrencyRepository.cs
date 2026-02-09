using Cysharp.Threading.Tasks;
using UnityEngine;

public class LocalCurrencyRepository : ICurrencyRepository
{
    private readonly string _userId;
    public LocalCurrencyRepository(string userId)
    {
        _userId = userId;
    }

    private string GetKey(ECurrencyType type) => $"{_userId}_{type}";

    // 재화 데이터 저장
    public UniTask Save(CurrencySaveData saveData)
    {
        for (int i = 0; i < (int)ECurrencyType.Count; i++)
        {
            var type = (ECurrencyType)i;
            PlayerPrefs.SetString(GetKey(type), saveData.Currencies[i].ToString("G17"));
        }
        return UniTask.CompletedTask;
    }

    // 재화 데이터 로드
    public UniTask<CurrencySaveData> Load()
    {
        // 일단 기본값으로 초기화
        CurrencySaveData data = CurrencySaveData.Default;

        // 모든 재화 종류를 순회
        for (int i = 0; i < (int)ECurrencyType.Count; i++)
        {
            var type = (ECurrencyType)i;
            string key = GetKey(type);

            if (PlayerPrefs.HasKey(key))
            {
                data.Currencies[i] = double.Parse(PlayerPrefs.GetString(key, "0"));
            }
        }
        return UniTask.FromResult(data);
    }
}

