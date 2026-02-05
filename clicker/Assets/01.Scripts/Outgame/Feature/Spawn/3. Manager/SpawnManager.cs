using Cysharp.Threading.Tasks;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    private ISpawnRepository _repository;

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
        Vector2[] directions = ToVector2Array(saveData.MeteorDirections);
        SpawnController.Instance.Initialize(saveData.RocketTimes, directions);
    }

    // ── 저장 ──

    private async UniTask Save()
    {
        var data = new SpawnSaveData
        {
            RocketTimes = SpawnController.Instance.GetRocketTimes(),
            MeteorDirections = ToFloatArray(SpawnController.Instance.GetMeteorDirections())
        };

        await _repository.Save(data);
    }

    // ── Vector2 변환 ──

    private static float[] ToFloatArray(Vector2[] vectors)
    {
        if (vectors == null) return null;

        float[] result = new float[vectors.Length * 2];
        for (int i = 0; i < vectors.Length; i++)
        {
            result[i * 2] = vectors[i].x;
            result[i * 2 + 1] = vectors[i].y;
        }
        return result;
    }

    private static Vector2[] ToVector2Array(float[] floats)
    {
        if (floats == null || floats.Length % 2 != 0) return null;

        Vector2[] result = new Vector2[floats.Length / 2];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = new Vector2(floats[i * 2], floats[i * 2 + 1]);
        }
        return result;
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
