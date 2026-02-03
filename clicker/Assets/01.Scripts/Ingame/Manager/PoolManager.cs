using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    private Dictionary<GameObject, Queue<GameObject>> _pools = new();
    private Dictionary<GameObject, GameObject> _instanceToPrefab = new();

    private void Awake()
    {
        Instance = this;
    }

    public void Preload(GameObject prefab, int count)
    {
        if (!_pools.ContainsKey(prefab))
        {
            _pools[prefab] = new Queue<GameObject>();
        }

        for (int i = 0; i < count; i++)
        {
            GameObject obj = CreateNewObject(prefab);
            obj.SetActive(false);
            _pools[prefab].Enqueue(obj);
        }
    }

    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!_pools.ContainsKey(prefab))
        {
            _pools[prefab] = new Queue<GameObject>();
        }

        GameObject obj;

        if (_pools[prefab].Count > 0)
        {
            obj = _pools[prefab].Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);
        }
        else
        {
            obj = CreateNewObject(prefab);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
        }

        return obj;
    }

    public void Despawn(GameObject obj, float delay = 0f)
    {
        if (delay > 0f)
        {
            StartCoroutine(DespawnCoroutine(obj, delay));
            return;
        }

        DespawnImmediate(obj);
    }

    private System.Collections.IEnumerator DespawnCoroutine(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        DespawnImmediate(obj);
    }

    private void DespawnImmediate(GameObject obj)
    {
        if (_instanceToPrefab.TryGetValue(obj, out GameObject prefab))
        {
            obj.SetActive(false);
            _pools[prefab].Enqueue(obj);
        }
        else
        {
            Destroy(obj);
        }
    }

    private GameObject CreateNewObject(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, transform);
        _instanceToPrefab[obj] = prefab;
        return obj;
    }
}
