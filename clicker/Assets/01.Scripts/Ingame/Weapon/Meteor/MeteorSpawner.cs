using UnityEngine;
using UnityEngine.Serialization;

public class MeteorSpawner : MonoBehaviour
{
    [FormerlySerializedAs("_rocks")]
    [SerializeField] private GameObject[] _meteors;

    [Header("Spawn Area")]
    [SerializeField] private Transform _parent;
    [SerializeField] private float _spawnRadius = 0.3f;

    public void Spawn()
    {
        Vector3 spawnPosition = GetRandomCircleWorldPosition(_parent.position);
        GameObject meteorPrefab = _meteors[Random.Range(0, _meteors.Length)];
        Instantiate(meteorPrefab, spawnPosition, Quaternion.identity, _parent);
    }

    private Vector3 GetRandomCircleWorldPosition(Vector3 centerWorld)
    {
        float angle = Random.value * Mathf.PI * 2f;
        float x = Mathf.Cos(angle) * _spawnRadius;
        float y = Mathf.Sin(angle) * _spawnRadius;

        return new Vector3(centerWorld.x + x, centerWorld.y + y, 0f);
    }
}
