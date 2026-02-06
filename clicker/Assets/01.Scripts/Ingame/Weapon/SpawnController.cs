using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public static SpawnController Instance { get; private set; }

    private RocketSpawner _rocketSpawner;
    private MeteorSpawner _meteorSpawner;
    private MiniPlanetSpawner _miniPlanetSpawner;

    public bool IsReady => _rocketSpawner != null && _meteorSpawner != null && _miniPlanetSpawner != null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        SpawnManager.Instance.OnDataInitiailized += Initialize;
        SpawnManager.Instance.OnSaveRequest += () =>
        {
            SpawnManager.Instance.Set(_rocketSpawner.GetTimes(), _meteorSpawner.GetDirections());
        };
    }

    private void Start()
    {
        UpgradeManager.OnUpgraded += OnUpgraded;
        StageController.Instance.OnStageChanged += OnStageChanged;
    }

    private void OnDestroy()
    {
        UpgradeManager.OnUpgraded -= OnUpgraded;
        StageController.Instance.OnStageChanged -= OnStageChanged;
    }

    // ── 등록 ──

    public void RegisterRocketSpawner(RocketSpawner spawner) => _rocketSpawner = spawner;
    public void RegisterMeteorSpawner(MeteorSpawner spawner) => _meteorSpawner = spawner;
    public void RegisterMiniPlanetSpawner(MiniPlanetSpawner spawner) => _miniPlanetSpawner = spawner;

    // ── 초기화 (SpawnManager가 호출) ──

    private void Initialize()
    {
        float[] rocketTimes = SpawnManager.Instance.Spawn.RocketTimes;

        Vector2[] meteorDirections = SpawnManager.Instance.Spawn.VectorMeteorDirections;

        // 로켓
        int rocketCount = UpgradeManager.Instance.GetUpgrade(EUpgradeEffect.RocketCount)?.Level ?? 0;
        for (int i = 0; i < rocketCount; i++)
            _rocketSpawner.Spawn();

        if (rocketTimes != null && rocketTimes.Length == rocketCount)
            _rocketSpawner.SetTimes(rocketTimes);
        else
            _rocketSpawner.Redistribute();

        // 운석
        int meteorCount = UpgradeManager.Instance.GetUpgrade(EUpgradeEffect.MeteorCount)?.Level ?? 0;
        for (int i = 0; i < meteorCount; i++)
            _meteorSpawner.Spawn();

        if (meteorDirections != null && meteorDirections.Length == meteorCount)
            _meteorSpawner.SetDirections(meteorDirections);

        // 미니행성
        int stage = PlanetManager.Instance.CurrentPlanet.Level;
        for (int i = 2; i <= stage; i++)
            _miniPlanetSpawner.Spawn(StageController.Instance.GetMiniSprite(i));
    }

    // ── 실시간 이벤트 ──

    private void OnUpgraded(EUpgradeEffect effect)
    {
        switch (effect)
        {
            case EUpgradeEffect.RocketCount:
                _rocketSpawner.Spawn();
                break;
            case EUpgradeEffect.MeteorCount:
                _meteorSpawner.Spawn();
                break;
        }
    }

    private void OnStageChanged(int newStage)
    {
        _miniPlanetSpawner.Spawn(StageController.Instance.PreviousSprite);
    }


}
