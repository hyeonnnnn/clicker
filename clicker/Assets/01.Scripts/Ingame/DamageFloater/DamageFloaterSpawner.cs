using Lean.Pool;
using UnityEngine;

public class DamageFloaterSpawner : MonoBehaviour
{
    public static DamageFloaterSpawner Instance { get; private set; }

    [SerializeField] private LeanGameObjectPool _manualPool;
    [SerializeField] private LeanGameObjectPool _autoPool;
    [SerializeField] private Vector2 _manualSpawnOffset;
    [SerializeField] private Vector2 _autoSpawnOffset;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowDamage(ClickInfo clickInfo)
    {
        if (clickInfo.Type == EClickType.Manual) ShowManualDamage(clickInfo);
        else if (clickInfo.Type == EClickType.Auto) ShowAutoDamage(clickInfo);
    }

    private void ShowManualDamage(ClickInfo clickInfo)
    {
        Vector3 spawnPosition = (Vector3)clickInfo.Position + (Vector3)_manualSpawnOffset;

        GameObject damageFloaterObject = _manualPool.Spawn(spawnPosition, Quaternion.identity);
        DamageFloater damageFloater = damageFloaterObject.GetComponent<DamageFloater>();

        damageFloater.ShowManual(clickInfo);
    }

    private void ShowAutoDamage(ClickInfo clickInfo)
    {
        Vector3 spawnPosition = (Vector3)clickInfo.Position + (Vector3)_autoSpawnOffset;

        GameObject damageFloaterObject = _autoPool.Spawn(spawnPosition, Quaternion.identity);
        DamageFloater damageFloater = damageFloaterObject.GetComponent<DamageFloater>();

        damageFloater.ShowAuto(clickInfo);
    }
}
