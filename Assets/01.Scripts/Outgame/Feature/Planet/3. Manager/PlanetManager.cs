using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    public static PlanetManager Instance { get; private set; }

    [SerializeField] private PlanetSpecTableSO _specTable;
    private HybridRepository<PlanetSaveData> _repository;

    private Planet _planet;

    public static event Action OnDataChanged;
    public static event Action<double, double> OnPressureChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _repository = new HybridRepository<PlanetSaveData>(
            new LocalPlanetRepository(),
#if !UNITY_WEBGL || UNITY_EDITOR
            new FirebasePlanetRepository()
#else
            null
#endif
        );
        InitializePlanets().Forget();
    }

    private async UniTask InitializePlanets()
    {
        var saveData = await _repository.Load();
        _planet = new Planet(_specTable.Datas[0], saveData.CurrentStage, saveData.CurrentPressure);
        OnDataChanged?.Invoke();
    }

    // ── 조회 ──
    public Planet CurrentPlanet => _planet;

    // ── 비즈니스 로직 ──
    public void UpdatePressure(double pressure)
    {
        _planet.UpdatePressure(pressure);
        OnPressureChanged?.Invoke(_planet.CurrentPressure, _planet.MaxPressure);
    }

    public void NextStage()
    {
        _planet.NextLevel();

        Save();
        OnDataChanged?.Invoke();
    }

    // ── 저장/불러오기 ──
    private void Save()
    {
        _repository.Save(new PlanetSaveData
        {
            CurrentStage = _planet.Level,
            CurrentPressure = _planet.CurrentPressure
        });
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Save();
            _repository.ForceRemoteSave().Forget();
        }
    }

    private void OnApplicationQuit()
    {
        Save();
        _repository.ForceRemoteSave().Forget();
    }
}
