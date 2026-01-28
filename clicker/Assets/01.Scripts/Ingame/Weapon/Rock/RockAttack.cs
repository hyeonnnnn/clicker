using UnityEngine;
using static EffectSpawner;
using static SoundManager;

public class RockAttack : MonoBehaviour
{
    [SerializeField] private double _damage = 1;

    private RockMove _rockMove;
    private Collider2D _collider;

    private void Awake()
    {
        _rockMove = GetComponent<RockMove>();
        _collider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Planet")) return;

        if (collision.collider.TryGetComponent(out PlanetPressure planetHealth))
        {
            planetHealth.TakeDamage(_damage);
        }

        // 물리 정보 추출
        ContactPoint2D contact = collision.contacts[0];
        Vector2 contactPoint = contact.point;
        Vector2 normal = contact.normal;

        ApplyDamage(collision.collider);
        SpawnImpactEffect(contactPoint, normal);
        ShowDamageText(contactPoint);

        SoundManager.Instance.PlaySFX(Sfx.ROCK);

        _rockMove.BounceFromCollision(normal);
    }

    private void ApplyDamage(Collider2D planetCollider)
    {
        if (planetCollider.TryGetComponent(out PlanetPressure planetPressure))
        {
            planetPressure.TakeDamage(_damage);
        }
    }

    private void SpawnImpactEffect(Vector2 position, Vector2 normal)
    {
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
        EffectSpawner.Instance.PlayEffect(Effect.ROCKATTACK, position, rotation);
    }

    private void ShowDamageText(Vector2 position)
    {
        TextFloaterSpawner.Instance.ShowDamage(new ClickInfo
        {
            Type = EClickType.Auto,
            Damage = _damage,
            Position = position
        });
    }
}
