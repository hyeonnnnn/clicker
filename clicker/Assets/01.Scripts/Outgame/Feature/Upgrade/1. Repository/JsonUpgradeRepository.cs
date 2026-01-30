using UnityEngine;
using System.IO;

public class JsonUpgradeRepository : IUpgradeRepository
{
    private readonly string filePath;

    public JsonUpgradeRepository()
    {
        filePath = Path.Combine(Application.persistentDataPath, "upgrade_save.json");
    }

    public void Save(UpgradeSaveData data)
    {
        data.LastSaveTime = System.DateTime.Now.ToString("o");
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }

    public UpgradeSaveData Load()
    {
        if (!File.Exists(filePath))
            return UpgradeSaveData.Default;

        string json = File.ReadAllText(filePath);
        return JsonUtility.FromJson<UpgradeSaveData>(json);
    }
}
