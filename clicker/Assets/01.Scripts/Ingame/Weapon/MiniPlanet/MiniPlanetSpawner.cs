using UnityEngine;

public class MiniPlanetSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _miniPlanetPrefab;
    private MiniPlanetAttackSequence _attackSequence;
    private MiniPlanetMove _miniPlanetMove;

    private void Awake()
    {
        _attackSequence = GetComponent<MiniPlanetAttackSequence>();
        _miniPlanetMove = GetComponent<MiniPlanetMove>();
    }

    public void Spawn(Sprite sprite)
    {
        var miniPlanet = Instantiate(_miniPlanetPrefab, transform);

        var spriteRenderer = miniPlanet.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = sprite;
        }

        var controller = miniPlanet.GetComponent<MiniPlanetController>();
        if (controller != null)
        {
            _attackSequence.AddMiniPlanet(controller);
        }

        _miniPlanetMove.Rearrange();
    }
}
