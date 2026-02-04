using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    [SerializeField] private RocketSpawner _rocketSpawner;
    [SerializeField] private MeteorSpawner _meteorSpawner;
    [SerializeField] private MiniPlanetSpawner _miniPlanetSpawner;

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
        StageManager.Instance.OnStageChanged += OnStageChanged;
    }

    private void OnDestroy()
    {
        UpgradeManager.OnUpgraded -= OnUpgraded;

        if (StageManager.Instance != null)
        {
            StageManager.Instance.OnStageChanged -= OnStageChanged;
        }
    }

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
        _miniPlanetSpawner.Spawn(StageManager.Instance.PreviousSprite);
    }
}
