using System;
using UnityEngine;

public class RocketMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _curveScale = 3f;
    [SerializeField] private float _patrolSpeed = 2f;
    [SerializeField] private float _attackBoostSpeed = 8f;
    [SerializeField] private float _smoothTime = 0.01f;

    [Header("Thresholds")]
    [SerializeField] private float _boostThreshold = 0.3f;
    [SerializeField] private float _centerPassDistance = 3f;

    private Transform _target;
    private Vector3 _currentVelocity;
    private float _currentTime;
    private bool _hasPassedCenter;

    public float CurrentTime => _currentTime;

    public event Action OnPassedCenter;

    public void Initialize(Transform target)
    {
        _target = target;
    }

    // 저장된 시간을 복구한다.
    public void SetTimeOffset(float offset)
    {
        _currentTime = offset;
    }

    private void Update()
    {
        if (_target == null) return;

        // 속도와 상태 계산
        float timeStep = CalculateTimeStep();
        _currentTime += timeStep;

        // 8자 궤도 계산
        Vector3 targetPosition = CalculateTargetPosition(_currentTime);

        // 이동 적용
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, _smoothTime);

        // 회전
        RotateTowardVelocity();
    }

    private float CalculateTimeStep()
    {
        float distFactor = Mathf.Abs(Mathf.Sin(_currentTime));
        bool isNearCenter = distFactor < _boostThreshold;

        if (isNearCenter)
        {
            CheckCenterPass();
            return Time.deltaTime * _attackBoostSpeed;
        }
        else
        {
            if (_hasPassedCenter) _hasPassedCenter = false;
            return Time.deltaTime * GetPatrolSpeed();
        }
    }

    private Vector3 CalculateTargetPosition(float time)
    {
        float x = Mathf.Sin(time) * _curveScale;
        float y = Mathf.Sin(2f * time) * (_curveScale * 0.5f);

        return _target.position + new Vector3(x, y, 0f);
    }

    private void CheckCenterPass()
    {
        float distToCenter = Vector3.Distance(transform.position, _target.position);

        if (!_hasPassedCenter && distToCenter < _centerPassDistance)
        {
            _hasPassedCenter = true;
            OnPassedCenter?.Invoke();
        }
    }

    private float GetPatrolSpeed()
    {
        var upgrade = UpgradeManager.Instance?.GetUpgrade(EUpgradeEffect.RocketCooldown);
        if (upgrade == null || upgrade.Level == 0) return _patrolSpeed;
        return _patrolSpeed + (float)(upgrade.BaseValue * upgrade.Level);
    }

    private void RotateTowardVelocity()
    {
        if (_currentVelocity.sqrMagnitude < 0.01f) return;

        float angle = Mathf.Atan2(_currentVelocity.y, _currentVelocity.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
