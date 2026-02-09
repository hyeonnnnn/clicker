using Firebase.Firestore;
using System;

[Serializable]
[FirestoreData]
public class CurrencySaveData : ISaveData
{
    // 재화 배열
    [FirestoreProperty]
    public double[] Currencies { get; set; }

    [FirestoreProperty]
    public string LastSavedAt { get; set; }

    // 재화 기본값
    public static CurrencySaveData Default => new CurrencySaveData()
    {
        Currencies = new double[(int)ECurrencyType.Count],
        LastSavedAt = null
    };
}
