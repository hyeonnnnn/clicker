using System;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    [SerializeField] private PlanetInfo _planetInfo;
    [SerializeField] private SpriteRenderer _planetRenderer;
    [SerializeField] private PlanetPressure _planetPressure;

    private int _previousStage;

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
        _planetPressure.OnDepleted += NextStage;
        PlanetManager.OnDataChanged += OnPlanetDataChanged;

        if (PlanetManager.Instance.CurrentPlanet != null)
        {
            InitializeStage();
        }
    }

    private void OnDestroy()
    {
        _planetPressure.OnDepleted -= NextStage;
        PlanetManager.OnDataChanged -= OnPlanetDataChanged;
    }

    private void OnPlanetDataChanged()
    {
        InitializeStage();
    }

    public void InitializeStage()
    {
        var planetData = CurrentPlanetData;
        _planetRenderer.sprite = planetData.Sprite;
        var planet = PlanetManager.Instance.CurrentPlanet;
        _planetPressure.Initialize(planet.MaxPressure, planet.CurrentPressure);
    }

    private void NextStage()
    {
        _previousStage = CurrentStage;
        PlanetManager.Instance.NextStage();
        OnStageChanged?.Invoke(CurrentStage);
    }
}
