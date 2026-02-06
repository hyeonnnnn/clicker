using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class WeaponStateManager : MonoBehaviour
{
    public static WeaponStateManager Instance { get; private set; }

    private IWeaponStateRepository _repository;

    public WeaponState weaponState;

    public Action OnDataInitiailized;
    public Action OnSaveRequest;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _repository = new FirebaseWeaponStateRepository();
    }

    private async UniTaskVoid Start()
    {
        await UniTask.WaitUntil(() =>
            SpawnController.Instance != null &&
            SpawnController.Instance.IsReady &&
            UpgradeManager.Instance.IsInitialized &&
            PlanetManager.Instance.CurrentPlanet != null);

        await Initialize();
    }

    // ── 초기화 ──

    private async UniTask Initialize()
    {
        WeaponStateSaveData saveData = await _repository.Load();

        weaponState = new WeaponState(saveData.RocketTimes, saveData.MeteorCount);

        OnDataInitiailized?.Invoke();
    }

    // ── 저장 ──
    public void Set(float[] rocketTimes, int meteorCount)
    {
        weaponState.SetRocketLaunchTimes(rocketTimes);
        weaponState.SetMeteorCount(meteorCount);
    }

    private async UniTask Save()
    {
        OnSaveRequest?.Invoke();

        var data = new WeaponStateSaveData
        {
            RocketTimes = weaponState.RocketLaunchTimes,
            MeteorCount = weaponState.MeteorCount
        };

        await _repository.Save(data);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) Save().Forget();
    }

    private void OnApplicationQuit()
    {
        Save().Forget();
    }
}
