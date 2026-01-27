using Lean.Pool;
using UnityEngine;

public class DamageFloaterSpawner : MonoBehaviour
{
    public static DamageFloaterSpawner Instance { get; private set; }

    [SerializeField] private LeanGameObjectPool _pool;
    [SerializeField] private Vector2 _spawnOffset;

    private void Awake()
    {
        Instance = this;
        _pool = GetComponent<LeanGameObjectPool>();
    }

    public void ShowDamage(ClickInfo clickInfo)
    {
        Vector3 spawnPosition = (Vector3)clickInfo.Position + (Vector3)_spawnOffset;

        GameObject damageFloaterObject = _pool.Spawn(spawnPosition, Quaternion.identity);
        DamageFloater damageFloater = damageFloaterObject.GetComponent<DamageFloater>();

        damageFloater.Show(clickInfo);
    }
}
