using System;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    [SerializeField] private PlanetInfo _planetInfo;
    [SerializeField] private SpriteRenderer _planetRenderer;
    [SerializeField] private PlanetHealth _planetHealth;

    private int _currentStage;

    public int CurrentStage => _currentStage;
    public Sprite CurrentSprite => _planetInfo.GetPlanet(_currentStage).Sprite;

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
        _planetHealth.OnDepleted += NextStage;
        InitializeStage(0);
    }

    private void OnDestroy()
    {
        _planetHealth.OnDepleted -= NextStage;
    }

    public void InitializeStage(int stageIndex)
    {
        _currentStage = Mathf.Clamp(stageIndex, 0, _planetInfo.Count - 1);
        var planetData = _planetInfo.GetPlanet(_currentStage);

        _planetRenderer.sprite = planetData.Sprite;
        _planetHealth.Initialize(planetData.Health);

        OnStageChanged?.Invoke(_currentStage);
    }

    private void NextStage()
    {
        if (_currentStage < _planetInfo.Count - 1)
        {
            InitializeStage(_currentStage + 1);
        }
    }
}
