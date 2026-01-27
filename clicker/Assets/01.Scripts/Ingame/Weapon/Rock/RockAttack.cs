using System.Collections;
using UnityEngine;

public class RockAttack : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _ignoreDuration = 0.3f;

    private RockMove _rockMove;
    private Collider2D _collider;

    private void Awake()
    {
        _rockMove = GetComponent<RockMove>();
        _collider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Planet"))
        {
            if (collision.collider.TryGetComponent(out PlanetHealth planetHealth))
            {
                planetHealth.TakeDamage(_damage);
            }

            Vector2 contactPoint = collision.contacts[0].point;

            TextFloaterSpawner.Instance.ShowDamage(new ClickInfo
            {
                Type = EClickType.Auto,
                Damage = _damage,
                Position = contactPoint
            });

            Vector2 normal = collision.contacts[0].normal;
            _rockMove.BounceFromCollision(normal);

            StartCoroutine(IgnoreCollisionTemporarily(collision.collider));
        }
    }

    private IEnumerator IgnoreCollisionTemporarily(Collider2D other)
    {
        Physics2D.IgnoreCollision(_collider, other, true);
        yield return new WaitForSeconds(_ignoreDuration);
        Physics2D.IgnoreCollision(_collider, other, false);
    }
}
