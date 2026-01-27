using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _rocks;
    [SerializeField] private float _spawnMargin = 0.1f;
    [SerializeField] private float _spawnDepth = 10f;

    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;

        // 테스트용
        SpawnRock();
        SpawnRock();
        SpawnRock();
        SpawnRock();
        SpawnRock();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SpawnRock();
        }
    }

    public void SpawnRock()
    {
        Vector3 spawnPosition = GetRandomScreenPosition();
        GameObject rockPrefab = _rocks[Random.Range(0, _rocks.Length)];
        Instantiate(rockPrefab, spawnPosition, Quaternion.identity);
    }

    private Vector3 GetRandomScreenPosition()
    {
        float x = Random.Range(_spawnMargin, 1f - _spawnMargin);
        float y = Random.Range(_spawnMargin, 1f - _spawnMargin);
        Vector3 viewportPos = new Vector3(x, y, _spawnDepth);
        return _camera.ViewportToWorldPoint(viewportPos);
    }
}
