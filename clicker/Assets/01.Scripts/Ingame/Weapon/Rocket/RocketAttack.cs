using UnityEngine;
using static EffectSpawner;
using static SoundManager;

public class RocketAttack : MonoBehaviour
{
    private double Damage => UpgradeManager.Instance.GetUpgrade(EUpgradeEffect.RocketPower)?.Value ?? 0;

    private Transform _target;
    private PlanetPressure _planetPressure;

    public void Initialize(Transform target, PlanetPressure planetPressure)
    {
        _target = target;
        _planetPressure = planetPressure;
    }

    public void Attack()
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
