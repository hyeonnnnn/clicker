using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Firestore;
using System;
using UnityEngine;

public class FirebaseUpgradeRepository : IUpgradeRepository
{
    private string UPGRADE_COLLECTION_NAME = "Upgrade";
    private FirebaseAuth _auth = FirebaseAuth.DefaultInstance;
    private FirebaseFirestore _db = FirebaseFirestore.DefaultInstance;

    public async UniTask Save(UpgradeSaveData data)
    {
        try
        {
            string email = _auth.CurrentUser.Email;

            await _db.Collection(UPGRADE_COLLECTION_NAME).Document(email).SetAsync(data);
        }
        catch (Exception e)
        {
            Debug.LogError("Upgrade 저장 실패: " + e.Message);
        }
    }

    public async UniTask<UpgradeSaveData> Load()
    {
        try
        {
            string email = _auth.CurrentUser.Email;

            DocumentSnapshot snapshot = await _db.Collection(UPGRADE_COLLECTION_NAME).Document(email).GetSnapshotAsync();

            // 처음 접속한 유저처럼 데이터가 없을 때는 디폴트 세이브 데이터 주기
            if (!snapshot.Exists)
                return UpgradeSaveData.Default;

            return snapshot.ConvertTo<UpgradeSaveData>();
        }
        catch (Exception e)
        {
            Debug.LogError("Upgrade 로드 실패: " + e.Message);
        }

        return UpgradeSaveData.Default;
    }
}
