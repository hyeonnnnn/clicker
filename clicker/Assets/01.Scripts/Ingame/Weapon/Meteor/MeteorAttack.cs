using UnityEngine;
using static EffectSpawner;
using static SoundManager;

public class MeteorAttack : MonoBehaviour
{
    private double Damage => UpgradeManager.Instance.GetUpgrade(EUpgradeEffect.MeteorPower)?.Value ?? 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Planet")) return;

        ContactPoint2D contact = collision.contacts[0];

        if (collision.collider.TryGetComponent(out PlanetPressure planetPressure))
        {
            planetPressure.TakeDamage(Damage);
        }

        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);

        WeaponFeedback.PlayImpact(
            new ClickInfo { Type = EClickType.Auto, Damage = Damage, Position = contact.point },
            Effect.METEORATTACK,
            Sfx.METEOR,
            contact.point,
            rotation
        );
    }
}
