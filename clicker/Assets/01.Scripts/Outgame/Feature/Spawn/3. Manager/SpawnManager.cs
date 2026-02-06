using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    private ISpawnRepository _repository;

    public Spawn Spawn;

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
        _repository = new FirebaseSpawnRepository();
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
        SpawnSaveData saveData = await _repository.Load();

        Spawn = new Spawn(saveData.RocketTimes, saveData.MeteorDirections);

        OnDataInitiailized?.Invoke();
    }

    // ── 저장 ──
    public void Set(float[] rocketTimes, Vector2[] meteorDirections)
    {
        Spawn.SetRocketTimes(rocketTimes);
        Spawn.SetMeteorDirections(meteorDirections);
    }



    private async UniTask Save()
    {
        OnSaveRequest?.Invoke();

        var data = new SpawnSaveData
        {
            RocketTimes = Spawn.RocketTimes,
            MeteorDirections = Spawn.MeteorDirections
        };

        await _repository.Save(data);
    }

    // ── Vector2 변환 ──

    

    private void OnApplicationPause(bool pause)
    {
        if (pause) Save().Forget();
    }

    private void OnApplicationQuit()
    {
        Save().Forget();
    }
}
