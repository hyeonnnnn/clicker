using UnityEngine;

public class ShurikenRotation : MonoBehaviour
{
    [SerializeField] private float _radius = 2f;
    [SerializeField] private float _rotationSpeed = 90f;

    private void Start()
    {
        ArrangeChildrenInCircle();
    }

    private void Update()
    {
        transform.Rotate(0f, 0f, _rotationSpeed * Time.deltaTime);
    }

    private void ArrangeChildrenInCircle()
    {
        int childCount = transform.childCount;
        if (childCount == 0) return;

        float angleStep = 360f / childCount;

        for (int i = 0; i < childCount; i++)
        {
            float angleDeg = angleStep * i;
            float angleRad = angleDeg * Mathf.Deg2Rad;

            Vector3 position = new Vector3(
                Mathf.Cos(angleRad) * _radius,
                Mathf.Sin(angleRad) * _radius,
                0f
            );

            Transform child = transform.GetChild(i);
            child.localPosition = position;
            child.localRotation = Quaternion.Euler(0f, 0f, angleDeg);
        }
    }
}
