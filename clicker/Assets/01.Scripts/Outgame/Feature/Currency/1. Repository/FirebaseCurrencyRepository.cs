using System;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;

public class FirebaseCurrencyRepository : ICurrencyRepository
{
    private string CURRENCY_COLLECTION_NAME = "Currency";
    private FirebaseAuth _auth = FirebaseAuth.DefaultInstance;
    private FirebaseFirestore _db = FirebaseFirestore.DefaultInstance;
    
    public async UniTask Save(CurrencySaveData saveData)
    {
        try
        {
            string email = _auth.CurrentUser.Email;

            await _db.Collection(CURRENCY_COLLECTION_NAME).Document(email).SetAsync(saveData);
        }
        catch (Exception e)
        {
            Debug.LogError("Currency 저장 실패: " + e.Message);
        }
    }

    public async UniTask<CurrencySaveData> Load()
    {
        try
        {
            string email = _auth.CurrentUser.Email;

            DocumentSnapshot snapshot = await _db.Collection(CURRENCY_COLLECTION_NAME).Document(email).GetSnapshotAsync();

            // 처음 접속한 유저처럼 데이터가 없을 때는 디폴트 세이브 데이터 주기
            if (!snapshot.Exists)
                return CurrencySaveData.Default;

            return snapshot.ConvertTo<CurrencySaveData>();
        }
        catch (Exception e)
        {
            Debug.LogError("Currency 로드 실패: " + e.Message);
        }

        return CurrencySaveData.Default;
    }
}
