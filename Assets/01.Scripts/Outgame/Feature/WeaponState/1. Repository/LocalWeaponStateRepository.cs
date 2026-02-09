using Cysharp.Threading.Tasks;
using UnityEngine;

public class LocalWeaponStateRepository : IWeaponStateRepository
{
    private readonly string _key;

    public LocalWeaponStateRepository(string userId)
    {
        _key = $"{userId}_weaponstate";
    }

    public UniTask Save(WeaponStateSaveData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(_key, json);
        PlayerPrefs.Save();
        return UniTask.CompletedTask;
    }

    public UniTask<WeaponStateSaveData> Load()
    {
        if (!PlayerPrefs.HasKey(_key))
            return UniTask.FromResult(WeaponStateSaveData.Default);

        string json = PlayerPrefs.GetString(_key);
        var data = JsonUtility.FromJson<WeaponStateSaveData>(json);
        return UniTask.FromResult(data);
    }
}