using Cysharp.Threading.Tasks;
using UnityEngine;

public class LocalPlanetRepository : IPlanetRepository
{
    private readonly string _key;

    public LocalPlanetRepository(string userId)
    {
        _key = $"{userId}_planet";
    }

    public UniTask Save(PlanetSaveData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(_key, json);
        PlayerPrefs.Save();
        return UniTask.CompletedTask;
    }

    public UniTask<PlanetSaveData> Load()
    {
        if (!PlayerPrefs.HasKey(_key))
            return UniTask.FromResult(PlanetSaveData.Default);

        string json = PlayerPrefs.GetString(_key);
        var data = JsonUtility.FromJson<PlanetSaveData>(json);
        return UniTask.FromResult(data);
    }
}