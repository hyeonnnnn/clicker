using UnityEngine;

public class LocalCurrencyRepository : ICurrencyRepository
{
    public void Save(CurrencySaveData saveData)
    {
        // 어떻게든 세이브한다.
        for (int i = 0; i < (int)ECurrencyType.Count; i++)
        {
            var type = (EClickType)i;
            PlayerPrefs.SetString(type.ToString(), saveData.Currencies[i].ToString("G17"));
        }
    }

    public CurrencySaveData Load()
    {
        CurrencySaveData data = CurrencySaveData.Default;

        for (int i = 0; i < (int)ECurrencyType.Count; i++)
        {
            if (PlayerPrefs.HasKey(i.ToString()))
            {
                data.Currencies[i] = double.Parse(PlayerPrefs.GetString(i.ToString(), "0"));
            }
        }
        return data;
    }

}

