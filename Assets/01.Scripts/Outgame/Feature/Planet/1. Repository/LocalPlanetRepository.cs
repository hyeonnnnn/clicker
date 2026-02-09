using Cysharp.Threading.Tasks;
using UnityEngine;

public class LocalPlanetRepository : IPlanetRepository
{
    private string Key => $"{AccountManager.Instance.Email}_planet";

    public UniTask Save(PlanetSaveData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(Key, json);
        PlayerPrefs.Save();
        return UniTask.CompletedTask;
    }

    public UniTask<PlanetSaveData> Load()
    {
        if (!PlayerPrefs.HasKey(Key))
            return UniTask.FromResult(PlanetSaveData.Default);

        string json = PlayerPrefs.GetString(Key);
        var data = JsonUtility.FromJson<PlanetSaveData>(json);
        return UniTask.FromResult(data);
    }
}