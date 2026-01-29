using System;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    [SerializeField] private PlanetInfo _planetInfo;
    [SerializeField] private SpriteRenderer _planetRenderer;
    [SerializeField] private PlanetPressure _planetPressure;

    private int _currentStage;
    private int _previousStage;

    public int CurrentStage => _currentStage;
    public PlanetData CurrentPlanetData => _planetInfo.GetPlanet(_currentStage);
    public Sprite CurrentSprite => CurrentPlanetData.Sprite;
    public Sprite PreviousSprite => _planetInfo.GetPlanet(_previousStage).MiniSprite;

    public event Action<int> OnStageChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _planetPressure.OnDepleted += NextStage;
        InitializeStage(0);
    }

    private void OnDestroy()
    {
        _planetPressure.OnDepleted -= NextStage;
    }

    public void InitializeStage(int stageIndex)
    {
        _currentStage = Mathf.Clamp(stageIndex, 0, _planetInfo.Count - 1);
        var planetData = _planetInfo.GetPlanet(_currentStage);

        _planetRenderer.sprite = planetData.Sprite;
        _planetPressure.Initialize(planetData.Pressure);
    }

    private void NextStage()
    {
        if (_currentStage < _planetInfo.Count - 1)
        {
            _previousStage = _currentStage;
            InitializeStage(_currentStage + 1);
            OnStageChanged?.Invoke(_currentStage);
        }
    }
}
