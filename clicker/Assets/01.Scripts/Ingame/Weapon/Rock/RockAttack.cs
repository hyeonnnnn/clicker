using UnityEngine;
using static EffectSpawner; // 가정된 네임스페이스
using static SoundManager;   // 가정된 네임스페이스

public class RockAttack : MonoBehaviour
{
    [SerializeField] private double _damage = 1;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Planet")) return;

        ContactPoint2D contact = collision.contacts[0];

        // 데미지 및 이펙트 처리
        ProcessAttack(collision.collider, contact);
    }

    private void ProcessAttack(Collider2D target, ContactPoint2D contact)
    {
        if (target.TryGetComponent(out PlanetPressure planetPressure))
        {
            planetPressure.TakeDamage(_damage);
        }

        SpawnImpactEffect(contact.point, contact.normal);
        ShowDamageText(contact.point);

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX(Sfx.ROCK);
    }

    private void SpawnImpactEffect(Vector2 position, Vector2 normal)
    {
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);

        if (EffectSpawner.Instance != null)
            EffectSpawner.Instance.PlayEffect(Effect.METEORATTACK, position, rotation);
    }

    private void ShowDamageText(Vector2 position)
    {
        if (TextFloaterSpawner.Instance != null)
        {
            TextFloaterSpawner.Instance.ShowDamage(new ClickInfo
            {
                Type = EClickType.Auto,
                Damage = _damage,
                Position = position
            });
        }
    }
}