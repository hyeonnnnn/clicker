using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public static SpawnController Instance { get; private set; }

    private RocketSpawner _rocketSpawner;
    private MeteorSpawner _meteorSpawner;
    private MiniPlanetSpawner _miniPlanetSpawner;

    private bool _isInitialized;

    public bool IsReady => _rocketSpawner != null && _meteorSpawner != null && _miniPlanetSpawner != null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        UpgradeManager.OnUpgraded += OnUpgraded;
        StageController.Instance.OnStageChanged += OnStageChanged;

        WeaponStateManager.Instance.OnDataInitiailized += Initialize;
        WeaponStateManager.Instance.OnSaveRequest += OnSaveRequest;
    }

    private void OnSaveRequest()
    {
        WeaponStateManager.Instance.Set(_rocketSpawner.GetTimes(), _meteorSpawner.GetCount());
    }

    private void OnDestroy()
    {
        UpgradeManager.OnUpgraded -= OnUpgraded;
        StageController.Instance.OnStageChanged -= OnStageChanged;
        WeaponStateManager.Instance.OnDataInitiailized -= Initialize;
        WeaponStateManager.Instance.OnSaveRequest -= OnSaveRequest;
    }

    // ── 등록 ──

    public void RegisterRocketSpawner(RocketSpawner spawner) => _rocketSpawner = spawner;
    public void RegisterMeteorSpawner(MeteorSpawner spawner) => _meteorSpawner = spawner;
    public void RegisterMiniPlanetSpawner(MiniPlanetSpawner spawner) => _miniPlanetSpawner = spawner;

    // ── 초기화 ──

    private void Initialize()
    {
        // 로켓
        int rocketCount = UpgradeManager.Instance.GetUpgrade(EUpgradeEffect.RocketCount)?.Level ?? 0;
        for (int i = 0; i < rocketCount; i++)
            _rocketSpawner.Spawn();

        float[] rocketTimes = WeaponStateManager.Instance.weaponState.RocketLaunchTimes;
        if (rocketTimes != null && rocketTimes.Length == rocketCount)
            _rocketSpawner.SetTimes(rocketTimes);
        else
            _rocketSpawner.Redistribute();

        // 운석
        int meteorCount = UpgradeManager.Instance.GetUpgrade(EUpgradeEffect.MeteorCount)?.Level ?? 0;
        for (int i = 0; i < meteorCount; i++)
            _meteorSpawner.Spawn();

        // 미니행성
        int stage = PlanetManager.Instance.CurrentPlanet.Level;
        int count = Mathf.Min(stage - 1, MiniPlanetSpawner.MAX_COUNT);
        for (int i = 1; i <= count; i++)
            _miniPlanetSpawner.Spawn(StageController.Instance.GetMiniSprite(i));

        _isInitialized = true;
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
        if (!_isInitialized)
            return;

        _miniPlanetSpawner.Spawn(StageController.Instance.PreviousSprite);
    }
}
