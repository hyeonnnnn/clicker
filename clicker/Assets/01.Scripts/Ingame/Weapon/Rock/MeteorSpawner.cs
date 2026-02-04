using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _rocks;

    [Header("Spawn Area")]
    [SerializeField] private Transform _parent;
    [SerializeField] private float _spawnRadius = 0.3f;

    private Camera _camera;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SpawnRock();
        }
    }

    public void SpawnRock()
    {
        Vector3 spawnPosition = GetRandomCircleWorldPosition(_parent.position);
        GameObject rockPrefab = _rocks[Random.Range(0, _rocks.Length)];
        Instantiate(rockPrefab, spawnPosition, Quaternion.identity, _parent);
    }

    private Vector3 GetRandomCircleWorldPosition(Vector3 centerWorld)
    {
        float angle = Random.value * Mathf.PI * 2f;
        float x = Mathf.Cos(angle) * _spawnRadius;
        float y = Mathf.Sin(angle) * _spawnRadius;

        return new Vector3(centerWorld.x + x, centerWorld.y + y, 0f);
    }
}
