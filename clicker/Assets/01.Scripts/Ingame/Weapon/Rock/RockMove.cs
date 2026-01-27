using DG.Tweening;
using UnityEngine;

public class RockMove : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _bounceRandomAngle = 30f;
    [SerializeField] private float _boostMultiplier = 2f;
    [SerializeField] private float _boostDuration = 0.3f;

    private Vector2 _direction;
    private Camera _camera;
    private float _currentSpeed;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _camera = Camera.main;
        _direction = Random.insideUnitCircle.normalized;
        _currentSpeed = _speed;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
        Bounce();

        _rigidbody.linearVelocity = Vector2.zero;
        _rigidbody.angularVelocity = 0f;
    }

    private void Move()
    {
        transform.Translate(_direction * _currentSpeed * Time.deltaTime);
    }

    private void Bounce()
    {
        Vector3 pos = transform.position;
        Vector3 viewportPos = _camera.WorldToViewportPoint(pos);

        bool bounced = false;

        if (viewportPos.x <= 0f || viewportPos.x >= 1f)
        {
            _direction.x = -_direction.x;
            bounced = true;
        }

        if (viewportPos.y <= 0f || viewportPos.y >= 1f)
        {
            _direction.y = -_direction.y;
            bounced = true;
        }

        if (bounced)
        {
            float randomAngle = Random.Range(-_bounceRandomAngle, _bounceRandomAngle);
            _direction = Rotate(_direction, randomAngle);
            SpeedBoost();
        }

        viewportPos.x = Mathf.Clamp(viewportPos.x, 0f, 1f);
        viewportPos.y = Mathf.Clamp(viewportPos.y, 0f, 1f);
        transform.position = _camera.ViewportToWorldPoint(viewportPos);
    }

    public void BounceFromCollision(Vector2 normal)
    {
        _direction = Vector2.Reflect(_direction, normal).normalized;
        float randomAngle = Random.Range(-_bounceRandomAngle, _bounceRandomAngle);
        _direction = Rotate(_direction, randomAngle);

        SpeedBoost();
    }

    private void SpeedBoost()
    {
        DOTween.Kill(this);
        _currentSpeed = _speed * _boostMultiplier;
        DOTween.To(() => _currentSpeed, x => _currentSpeed = x, _speed, _boostDuration)
            .SetEase(Ease.OutQuad)
            .SetTarget(this);
    }

    private Vector2 Rotate(Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
    }
}
