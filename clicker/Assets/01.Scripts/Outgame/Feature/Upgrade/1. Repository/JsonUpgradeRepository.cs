using UnityEngine;
using System.IO;

public class JsonUpgradeRepository : IUpgradeRepository
{

    private readonly string _filePath;

    // userId를 받아서 파일을 분리
    public JsonUpgradeRepository(string userId)
    {
        _filePath = Path.Combine(Application.persistentDataPath, $"{userId}_upgrade_save.json");
    }

    public void Save(UpgradeSaveData data)
    {
        data.LastSaveTime = System.DateTime.Now.ToString("o");
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(_filePath, json);
    }

    public UpgradeSaveData Load()
    {
        if (!File.Exists(_filePath))
            return UpgradeSaveData.Default;

        string json = File.ReadAllText(_filePath);
        return JsonUtility.FromJson<UpgradeSaveData>(json);
    }
}
