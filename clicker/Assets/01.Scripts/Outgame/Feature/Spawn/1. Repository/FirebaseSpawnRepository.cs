using System;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;

public class FirebaseSpawnRepository : ISpawnRepository
{
    private string SPAWN_COLLECTION_NAME = "Spawn";
    private FirebaseAuth _auth = FirebaseAuth.DefaultInstance;
    private FirebaseFirestore _db = FirebaseFirestore.DefaultInstance;

    public async UniTask Save(SpawnSaveData saveData)
    {
        try
        {
            string email = _auth.CurrentUser.Email;
            await _db.Collection(SPAWN_COLLECTION_NAME).Document(email).SetAsync(saveData);
        }
        catch (Exception e)
        {
            Debug.LogError("Spawn 저장 실패: " + e.Message);
        }
    }

    public async UniTask<SpawnSaveData> Load()
    {
        try
        {
            string email = _auth.CurrentUser.Email;
            DocumentSnapshot snapshot = await _db.Collection(SPAWN_COLLECTION_NAME).Document(email).GetSnapshotAsync();

            if (!snapshot.Exists)
                return SpawnSaveData.Default;

            return snapshot.ConvertTo<SpawnSaveData>();
        }
        catch (Exception e)
        {
            Debug.LogError("Spawn 로드 실패: " + e.Message);
        }

        return SpawnSaveData.Default;
    }
}
