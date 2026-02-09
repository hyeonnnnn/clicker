using Cysharp.Threading.Tasks;
using UnityEngine;

public class LocalWeaponStateRepository : IWeaponStateRepository
{
    private string Key => $"{AccountManager.Instance.Email}_weaponstate";

    public UniTask Save(WeaponStateSaveData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(Key, json);
        PlayerPrefs.Save();
        return UniTask.CompletedTask;
    }

    public UniTask<WeaponStateSaveData> Load()
    {
        if (!PlayerPrefs.HasKey(Key))
            return UniTask.FromResult(WeaponStateSaveData.Default);

        string json = PlayerPrefs.GetString(Key);
        var data = JsonUtility.FromJson<WeaponStateSaveData>(json);
        return UniTask.FromResult(data);
    }
}