using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    public static EffectSpawner Instance { get; private set; }

    [SerializeField] private float _defaultDespawnDelay = 2f;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject Spawn(GameObject prefab, Vector3 position)
    {
        return PoolManager.Instance.Spawn(prefab, position, Quaternion.identity);
    }

    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        return PoolManager.Instance.Spawn(prefab, position, rotation);
    }

    public void Despawn(GameObject obj)
    {
        PoolManager.Instance.Despawn(obj, _defaultDespawnDelay);
    }

    public void Despawn(GameObject obj, float delay)
    {
        PoolManager.Instance.Despawn(obj, delay);
    }
}
