using UnityEngine;

public class FirebaseCurrencyRepository : ICurrencyRepository
{
    public void Save(CurrencySaveData saveData)
    {
    }

    public CurrencySaveData Load()
    {
        return CurrencySaveData.Default;
    }
}
