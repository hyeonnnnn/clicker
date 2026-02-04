using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _speed = 100f; // 속도 값을 좀 더 키워야 할 수 있습니다 (초당 회전 각도)

    private void FixedUpdate()
    {
        _rigidbody.angularVelocity = -_speed;
    }
}