using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MeteorMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _baseSpeed = 5f;
    [SerializeField] private float _bounceRandomAngle = 30f;

    [Header("Boost Settings")]
    [SerializeField] private float _boostMultiplier = 2f;
    [SerializeField] private float _boostDuration = 0.3f;

    private Vector2 _direction;
    private float _currentSpeed;

    private Camera _mainCamera;
    private Rigidbody2D _rb;
    private Tween _speedTween;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        InitializePhysics();

        // 초기 방향 설정
        _direction = Random.insideUnitCircle.normalized;
        _currentSpeed = GetUpgradedSpeed();
    }

    private void FixedUpdate()
    {
        // 물리 이동 적용
        _rb.linearVelocity = _direction * _currentSpeed;
    }

    private void Update()
    {
        CheckScreenBoundaries();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollisionBounce(collision.contacts[0].normal);
    }

    private void InitializePhysics()
    {
        _rb.gravityScale = 0;
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        _rb.freezeRotation = true;
    }

    private void HandleCollisionBounce(Vector2 hitNormal)
    {
        _direction = Vector2.Reflect(_direction, hitNormal).normalized;

        transform.position += (Vector3)(hitNormal * 0.05f);

        ApplyRandomAngle();
        ApplySpeedBoost();
    }

    private void CheckScreenBoundaries()
    {
        if (_mainCamera == null) return;

        Vector3 viewportPos = _mainCamera.WorldToViewportPoint(transform.position);
        bool bounced = false;
        Vector2 normal = Vector2.zero;

        if (viewportPos.x <= 0f) { normal = Vector2.right; bounced = true; }
        else if (viewportPos.x >= 1f) { normal = Vector2.left; bounced = true; }

        if (viewportPos.y <= 0f) { normal = Vector2.up; bounced = true; }
        else if (viewportPos.y >= 1f) { normal = Vector2.down; bounced = true; }

        if (bounced)
        {
            viewportPos.x = Mathf.Clamp(viewportPos.x, 0.02f, 0.98f);
            viewportPos.y = Mathf.Clamp(viewportPos.y, 0.02f, 0.98f);
            transform.position = _mainCamera.ViewportToWorldPoint(viewportPos);

            if (normal != Vector2.zero)
            {
                _direction = Vector2.Reflect(_direction, normal).normalized;
                ApplyRandomAngle();
                ApplySpeedBoost();
            }
        }
    }

    private void ApplyRandomAngle()
    {
        float randomAngle = Random.Range(-_bounceRandomAngle, _bounceRandomAngle);
        _direction = RotateVector(_direction, randomAngle);
    }

    private void ApplySpeedBoost()
    {
        if (_speedTween != null && _speedTween.IsActive())
            _speedTween.Kill();

        float upgradedSpeed = GetUpgradedSpeed();
        float targetBoostSpeed = upgradedSpeed * _boostMultiplier;
        _currentSpeed = targetBoostSpeed;

        _speedTween = DOTween.To(() => _currentSpeed, x => _currentSpeed = x, upgradedSpeed, _boostDuration)
            .SetEase(Ease.OutQuad)
            .SetTarget(this);
    }

    private float GetUpgradedSpeed()
    {
        var upgrade = UpgradeManager.Instance?.GetUpgrade(EUpgradeEffect.MeteorSpeed);
        if (upgrade == null) return _baseSpeed;
        return _baseSpeed * (float)upgrade.Value;
    }

    private Vector2 RotateVector(Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
    }
}
