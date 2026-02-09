using Cysharp.Threading.Tasks;
using System.IO;
using UnityEngine;

public class JsonUpgradeRepository : IUpgradeRepository
{
    private string FilePath => Path.Combine(Application.persistentDataPath, $"{AccountManager.Instance.Email}_upgrade_save.json");

    public UniTask Save(UpgradeSaveData data)
    {
        data.LastSavedAt = System.DateTime.Now.ToString("o");
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(FilePath, json);

        return UniTask.CompletedTask;
    }

    public UniTask<UpgradeSaveData> Load()
    {
        if (!File.Exists(FilePath))
            return UniTask.FromResult(UpgradeSaveData.Default);

        string json = File.ReadAllText(FilePath);
        return UniTask.FromResult(JsonUtility.FromJson<UpgradeSaveData>(json));
    }
}
