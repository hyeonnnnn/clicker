using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _speed = 3f;

    private void FixedUpdate()
    {
        Rotation();
    }

    private void Rotation()
    {
        float nextAngle = _rigidbody.rotation - (_speed * Time.fixedDeltaTime);
        _rigidbody.MoveRotation(nextAngle);
    }
}
