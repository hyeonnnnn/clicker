using UnityEngine;
using static EffectSpawner;
using static SoundManager;

public class RocketAttack : MonoBehaviour
{
    private double Damage => UpgradeManager.Instance.GetUpgrade(EUpgradeEffect.RocketPower)?.Value ?? 0;

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
        _planetPressure.TakeDamage(Damage);

        var direction = (_target.position - transform.position).normalized;
        var rotation = Quaternion.FromToRotation(Vector3.up, direction);

        WeaponFeedback.PlayImpact(
            new ClickInfo { Type = EClickType.Auto, Damage = Damage, Position = _target.position },
            Effect.ROCKETATTACK,
            Sfx.SHURIKEN,
            _target.position,
            rotation
        );
    }
}
