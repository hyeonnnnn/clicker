using UnityEngine;
using static EffectSpawner;
using static SoundManager;

public class RocketAttack : MonoBehaviour
{
    [SerializeField] private int _damage = 20;

    private RocketMove _rocketMove;
    private Transform _target;
    private PlanetPressure _planetPressure;

    private void Awake()
    {
        _rocketMove = GetComponent<RocketMove>();
    }

    private void Start()
    {
        var planet = GameObject.FindGameObjectWithTag("Planet");
        if (planet != null)
        {
            _target = planet.transform;
            _planetPressure = planet.GetComponent<PlanetPressure>();
        }

        _rocketMove.OnPassedCenter += OnPassedCenter;
    }

    private void OnDestroy()
    {
        _rocketMove.OnPassedCenter -= OnPassedCenter;
    }

    private void OnPassedCenter()
    {
        HandleImpact();
    }

    private void HandleImpact()
    {
        _planetPressure.TakeDamage(_damage);

        TextFloaterSpawner.Instance.ShowDamage(new ClickInfo
        {
            Type = EClickType.Auto,
            Damage = _damage,
            Position = _target.position
        });

        var direction = (_target.position - transform.position).normalized;
        var rotation = Quaternion.FromToRotation(Vector3.up, direction);
        EffectSpawner.Instance.PlayEffect(Effect.ROCKETATTACK, _target.position, rotation);

        SoundManager.Instance.PlaySFX(Sfx.SHURIKEN);
    }
}
