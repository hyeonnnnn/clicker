using DG.Tweening;
using UnityEngine;

public class ShurikenAttack : MonoBehaviour
{
    [SerializeField] private PlanetPressure _planetPressure;
    [SerializeField] private Transform _target;
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _attackDuration = 0.2f;
    [SerializeField] private float _returnDuration = 0.3f;

    [Header("Effects")]
    [SerializeField] private GameObject _impactEffectPrefab;
    [SerializeField] private float _effectDespawnDelay = 1.5f;

    private Transform _parent;
    private Vector3 _localPosition;
    private Quaternion _localRotation;
    private bool _isAttacking;

    public void Initialize(Vector3 localPosition, Quaternion localRotation)
    {
        _parent = transform.parent;
        _localPosition = localPosition;
        _localRotation = localRotation;
    }

    public void Attack()
    {
        if (_isAttacking) return;

        _isAttacking = true;

        // 부모에서 분리
        transform.SetParent(null);

        // 행성 방향으로 Z축 회전
        Vector3 direction = _target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // 행성으로 이동
        transform.DOMove(_target.position, _attackDuration)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        HandleImpact(direction);

                        transform.SetParent(_parent);

                        Sequence returnSequence = DOTween.Sequence();
                        returnSequence.Join(transform.DOLocalMove(_localPosition, _returnDuration).SetEase(Ease.OutBack));
                        returnSequence.Join(transform.DOLocalRotateQuaternion(_localRotation, _returnDuration).SetEase(Ease.OutBack));
                        returnSequence.OnComplete(() => _isAttacking = false);
                    });
    }

    private void HandleImpact(Vector3 attackDirection)
    {
        _planetPressure.TakeDamage(_damage);

        TextFloaterSpawner.Instance.ShowDamage(new ClickInfo
        {
            Type = EClickType.Auto,
            Damage = _damage,
            Position = _target.position
        });

        Vector2 impactPoint = (Vector2)_target.position;
        Vector2 impactNormal = -(Vector2)attackDirection;

        SpawnImpactEffect(impactPoint, impactNormal);
    }

    private void SpawnImpactEffect(Vector2 position, Vector2 normal)
    {
        if (_impactEffectPrefab == null) return;

        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
        GameObject effect = EffectSpawner.Instance.Spawn(_impactEffectPrefab, position, rotation);
        EffectSpawner.Instance.Despawn(effect, _effectDespawnDelay);
    }
}
