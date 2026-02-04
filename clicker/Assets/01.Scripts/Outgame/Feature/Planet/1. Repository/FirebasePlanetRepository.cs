using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Firestore;
using System;
using UnityEngine;

public class FirebasePlanetRepository : IPlanetRepository
{
    private string PLANET_COLLECTION_NAME = "Planet";
    private FirebaseAuth _auth = FirebaseAuth.DefaultInstance;
    private FirebaseFirestore _db = FirebaseFirestore.DefaultInstance;

    public async UniTask Save(PlanetSaveData data)
    {
        try
        {
            string email = _auth.CurrentUser.Email;

            await _db.Collection(PLANET_COLLECTION_NAME).Document(email).SetAsync(data);
        }
        catch (Exception e)
        {
            Debug.LogError("Planet 저장 실패: " + e.Message);
        }
    }

    public async UniTask<PlanetSaveData> Load()
    {
        try
        {
            string email = _auth.CurrentUser.Email;

            DocumentSnapshot snapshot = await _db.Collection(PLANET_COLLECTION_NAME).Document(email).GetSnapshotAsync();

            // 처음 접속한 유저처럼 데이터가 없을 때는 디폴트 세이브 데이터 주기
            if (!snapshot.Exists)
                return PlanetSaveData.Default;

            return snapshot.ConvertTo<PlanetSaveData>();
        }
        catch (Exception e)
        {
            Debug.LogError("Planet 로드 실패: " + e.Message);
        }

        return PlanetSaveData.Default;
    }
}
