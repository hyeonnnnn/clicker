using UnityEngine;

public class MiniPlanetMove : MonoBehaviour
{
    [SerializeField] private PlanetInfo _planetInfo;
    [SerializeField] private float _minRadius = 2f;
    [SerializeField] private float _minSpacing = 1f;
    [SerializeField] private float _rotationSpeed = 90f;

    private int _totalSlots;
    private float _radius;
    private float _angleStep;

    private void Awake()
    {
        _totalSlots = _planetInfo.Count;
        _radius = CalculateRadius(_totalSlots);
        _angleStep = 360f / _totalSlots;
    }

    private void Update()
    {
        transform.Rotate(0f, 0f, _rotationSpeed * Time.deltaTime);
    }

    public void Rearrange()
    {
        int childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Vector3 position = GetSlotPosition(i);
            Quaternion rotation = GetSlotRotation(i);

            child.localPosition = position;
            child.localRotation = rotation;

            var attack = child.GetComponent<MiniPlanetAttack>();
            if (attack != null)
            {
                attack.Initialize(position, rotation);
            }
        }
    }

    private Vector3 GetSlotPosition(int slotIndex)
    {
        float angleDeg = _angleStep * slotIndex;
        float angleRad = angleDeg * Mathf.Deg2Rad;

        return new Vector3(
            Mathf.Cos(angleRad) * _radius,
            Mathf.Sin(angleRad) * _radius,
            0f
        );
    }

    private Quaternion GetSlotRotation(int slotIndex)
    {
        float angleDeg = _angleStep * slotIndex;
        return Quaternion.Euler(0f, 0f, angleDeg);
    }

    private float CalculateRadius(int count)
    {
        if (count <= 1) return _minRadius;

        float requiredRadius = (_minSpacing * count) / (2f * Mathf.PI);
        return Mathf.Max(_minRadius, requiredRadius);
    }
}
