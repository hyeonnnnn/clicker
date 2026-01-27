using DG.Tweening;
using UnityEngine;

public class ShurikenAttack : MonoBehaviour
{
    [SerializeField] private PlanetHealth _planetHealth;
    [SerializeField] private Transform _target;
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _attackDuration = 0.2f;
    [SerializeField] private float _returnDuration = 0.3f;

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
                // 데미지 입히기
                _planetHealth.TakeDamage(_damage);

                DamageFloaterSpawner.Instance.ShowDamage(new ClickInfo
                {
                    Type = EClickType.Auto,
                    Damage = _damage,
                    Position = _target.position
                });

                transform.SetParent(_parent);
                transform.DOLocalMove(_localPosition, _returnDuration)
                    .SetEase(Ease.OutBack);
                transform.DOLocalRotateQuaternion(_localRotation, _returnDuration)
                    .SetEase(Ease.OutBack)
                    .OnComplete(() => _isAttacking = false);
            });
    }
}
