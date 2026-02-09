using System;
using UnityEngine;

public class StageController : MonoBehaviour
{
    public static StageController Instance { get; private set; }

    [SerializeField] private PlanetInfo _planetInfo;
    [SerializeField] private SpriteRenderer _planetRenderer;

    private int _previousStage;
    private bool _isInitialized;

    public int CurrentStage => PlanetManager.Instance.CurrentPlanet.Level;
    public PlanetData CurrentPlanetData => _planetInfo.GetPlanet(CurrentStage % _planetInfo.Count);
    public Sprite CurrentSprite => CurrentPlanetData.Sprite;
    public Sprite PreviousSprite => _planetInfo.GetPlanet(_previousStage % _planetInfo.Count).MiniSprite;

    public event Action<int> OnStageChanged;

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
        PlanetManager.OnDataChanged += OnPlanetDataChanged;

        if (PlanetManager.Instance.CurrentPlanet != null)
        {
            _previousStage = CurrentStage;
            InitializeStage();
            _isInitialized = true;
        }
    }

    private void OnDestroy()
    {
        PlanetManager.OnDataChanged -= OnPlanetDataChanged;
    }

    private void OnPlanetDataChanged()
    {
        int newStage = CurrentStage;
        bool stageChanged = _previousStage != newStage;

        if (stageChanged)
        {
            SoundManager.Instance.PlaySFX(SoundManager.Sfx.POPPLANET);
            OnStageChanged?.Invoke(newStage);
        }

        _previousStage = newStage;
        InitializeStage();
    }

    public PlanetInfoViewData GetPlanetInfoViewData()
    {
        var data = CurrentPlanetData;
        return new PlanetInfoViewData
        {
            Name = data?.Name ?? "",
            Level = $"{PlanetManager.Instance.CurrentPlanet.Level}",
            Icon = data?.Icon
        };
    }

    public Sprite GetMiniSprite(int stage)
    {
        return _planetInfo.GetPlanet(stage % _planetInfo.Count).MiniSprite;
    }

    public void InitializeStage()
    {
        _planetRenderer.sprite = CurrentPlanetData.Sprite;
    }
}
