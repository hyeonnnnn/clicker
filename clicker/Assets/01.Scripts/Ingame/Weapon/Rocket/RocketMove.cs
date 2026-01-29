using System;
using UnityEngine;

public class RocketMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _curveScale = 3f;
    [SerializeField] private float _patrolSpeed = 2f;
    [SerializeField] private float _attackBoostSpeed = 8f;
    [SerializeField] private float _smoothTime = 0.1f;

    private Transform _target;
    private Vector3 _currentVelocity;
    private float _time;
    private bool _hasPassedCenter;

    public event Action OnPassedCenter;

    private void Start()
    {
        var planet = GameObject.FindGameObjectWithTag("Planet");
        if (planet != null)
        {
            _target = planet.transform;
        }
    }

    private void Update()
    {
        if (_target == null) return;

        float distFactor = Mathf.Abs(Mathf.Sin(_time));
        bool approachingCenter = distFactor < 0.3f;
        float targetSpeed = _patrolSpeed;

        if (approachingCenter)
        {
            targetSpeed = _attackBoostSpeed;
            CheckCenterPass();
        }

        _time += Time.deltaTime * targetSpeed;

        // 8자 궤도 계산
        float x = Mathf.Sin(_time) * _curveScale;
        float y = Mathf.Sin(2f * _time) * (_curveScale * 0.5f);

        Vector3 targetPos = _target.position + new Vector3(x, y, 0f);
        Vector3 newPos = Vector3.SmoothDamp(transform.position, targetPos, ref _currentVelocity, _smoothTime);
        transform.position = newPos;

        RotateTowardVelocity();
    }

    private void CheckCenterPass()
    {
        float distToCenter = Vector3.Distance(transform.position, _target.position);

        if (!_hasPassedCenter && distToCenter < 3f)
        {
            Debug.Log("CenterPass");
            _hasPassedCenter = true;
            OnPassedCenter?.Invoke();
        }
    }

    private void RotateTowardVelocity()
    {
        if (_currentVelocity.sqrMagnitude < 0.01f) return;

        float angle = Mathf.Atan2(_currentVelocity.y, _currentVelocity.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void SetMyTurn(bool isMyTurn)
    {
        if (isMyTurn)
        {
            _hasPassedCenter = false;
        }
    }
}
