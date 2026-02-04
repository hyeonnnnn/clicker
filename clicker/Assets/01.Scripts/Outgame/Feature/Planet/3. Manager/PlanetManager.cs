using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    public static PlanetManager Instance { get; private set; }

    [SerializeField] private PlanetSpecTableSO _specTable;
    private IPlanetRepository _repository;

    private Planet _planet;

    public static event Action OnDataChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _repository = new FirebasePlanetRepository();
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
    }

    public void NextStage()
    {
        _planet.NextLevel();

        Save().Forget();
        OnDataChanged?.Invoke();
    }

    // ── 저장/불러오기 ──
    private async UniTask Save()
    {
        await _repository.Save(new PlanetSaveData
        {
            CurrentStage = _planet.Level,
            CurrentPressure = _planet.CurrentPressure
        });
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
