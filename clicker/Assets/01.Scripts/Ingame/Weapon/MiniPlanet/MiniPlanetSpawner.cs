using UnityEngine;

public class MiniPlanetSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _miniPlanetPrefab;
    private MiniPlanetAttackSequence _attackerSelector;
    private MiniPlanetMove _miniPlanetMove;
    private Transform _spawnParent;

    private void Awake()
    {
        _attackerSelector = GetComponent<MiniPlanetAttackSequence>();
        _miniPlanetMove = GetComponent<MiniPlanetMove>();
        _spawnParent = transform;
    }

    private void Start()
    {
        StageManager.Instance.OnStageChanged += OnStageChanged;
    }

    private void OnDestroy()
    {
        if (StageManager.Instance != null)
        {
            StageManager.Instance.OnStageChanged -= OnStageChanged;
        }
    }

    private void OnStageChanged(int newStage)
    {
        SpawnMiniPlanet(StageManager.Instance.PreviousSprite);
    }

    private void SpawnMiniPlanet(Sprite sprite)
    {
        var miniPlanet = Instantiate(_miniPlanetPrefab, _spawnParent);

        var spriteRenderer = miniPlanet.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = sprite;
        }

        var attack = miniPlanet.GetComponent<MiniPlanetAttack>();
        if (attack != null)
        {
            _attackerSelector.AddMiniPlanet(attack);
        }

        _miniPlanetMove.Rearrange();
    }
}
