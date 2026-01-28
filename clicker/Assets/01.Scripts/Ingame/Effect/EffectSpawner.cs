using System.Collections.Generic;
using UnityEngine;

public class EffectSpawner : MonoBehaviour
{
    public static EffectSpawner Instance { get; private set; }

    [SerializeField] private float _defaultDespawnDelay = 2f;

    public enum Effect { CLICKPLANET, ROCKETATTACK, ROCKATTACK }

    [System.Serializable]
    public struct EffectData
    {
        public Effect type;
        public GameObject effect;
    }

    [SerializeField] private List<EffectData> _effectList;
    private Dictionary<Effect, EffectData> _effectDict = new Dictionary<Effect, EffectData>();

    private void Awake()
    {
        Instance = this;

        Initialize();
    }

    private void Initialize()
    {
        foreach (var data in _effectList)
        {
            _effectDict[data.type] = data;
        }
    }

    public GameObject PlayEffect(Effect effect, Vector3 position)
    {
        if (!_effectDict.TryGetValue(effect, out var data) || data.effect == null)
        {
            return null;
        }

        var obj = Spawn(data.effect, position);
        Despawn(obj);
        return obj;
    }

    public GameObject PlayEffect(Effect effect, Vector3 position, Quaternion rotation)
    {
        if (!_effectDict.TryGetValue(effect, out var data) || data.effect == null)
        {
            return null;
        }

        var obj = Spawn(data.effect, position, rotation);
        Despawn(obj);
        return obj;
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
