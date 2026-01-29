using DG.Tweening;
using UnityEngine;
using static EffectSpawner;
using static SoundManager;

public class MiniPlanetAttack : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _attackDuration = 0.2f;
    [SerializeField] private float _returnDuration = 0.3f;

    private GameObject _targetObject;
    private PlanetPressure _planetPressure;
    private Transform _targetTransform;

    private Transform _parent;
    private Vector3 _localPosition;
    private Quaternion _localRotation;
    private bool _isAttacking;

    private void Start()
    {
        _targetObject = GameObject.FindGameObjectWithTag("Planet");
        _targetTransform = _targetObject.transform;
        _planetPressure = _targetObject.GetComponent<PlanetPressure>();
    }

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
        Vector3 direction = _targetTransform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // 행성으로 이동
        transform.DOMove(_targetTransform.position, _attackDuration)
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

        SoundManager.Instance.PlaySFX(Sfx.SHURIKEN);
    }

    private void HandleImpact(Vector3 attackDirection)
    {
        _planetPressure.TakeDamage(_damage);

        TextFloaterSpawner.Instance.ShowDamage(new ClickInfo
        {
            Type = EClickType.Auto,
            Damage = _damage,
            Position = _targetTransform.position
        });

        Vector2 impactPoint = (Vector2)_targetTransform.position;
        Vector2 impactNormal = -(Vector2)attackDirection;

        SpawnImpactEffect(impactPoint, impactNormal);
    }

    private void SpawnImpactEffect(Vector2 position, Vector2 normal)
    {
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
        EffectSpawner.Instance.PlayEffect(Effect.ROCKETATTACK, position, rotation);
    }
}
