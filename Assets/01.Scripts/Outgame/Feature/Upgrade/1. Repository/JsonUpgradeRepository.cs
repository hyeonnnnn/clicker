using Cysharp.Threading.Tasks;
using System.IO;
using UnityEngine;

public class JsonUpgradeRepository : IUpgradeRepository
{
    private readonly string _filePath;

    public JsonUpgradeRepository(string userId)
    {
        _filePath = Path.Combine(Application.persistentDataPath, $"{userId}_upgrade_save.json");
    }

    public UniTask Save(UpgradeSaveData data)
    {
        data.LastSavedAt = System.DateTime.Now.ToString("o");
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(_filePath, json);

        return UniTask.CompletedTask;
    }

    public UniTask<UpgradeSaveData> Load()
    {
        if (!File.Exists(_filePath))
            return UniTask.FromResult(UpgradeSaveData.Default);

        string json = File.ReadAllText(_filePath);
        return UniTask.FromResult(JsonUtility.FromJson<UpgradeSaveData>(json));
    }
}
