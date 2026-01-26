using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private float _speed = 0.1f;

    private void Update()
    {
        Rotation();
    }

    private void Rotation()
    {
        _transform.Rotate(0f, 0f, -_speed * Time.deltaTime);
    }
}
