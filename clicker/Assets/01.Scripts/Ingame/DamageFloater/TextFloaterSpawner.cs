using Lean.Pool;
using UnityEngine;

public class TextFloaterSpawner : MonoBehaviour
{
    public static TextFloaterSpawner Instance { get; private set; }

    [Header("수동 공격 데미지")]
    [SerializeField] private LeanGameObjectPool _manualPool;
    [SerializeField] private Vector2 _manualSpawnOffset;

    [Header("자동 공격 데미지")]
    [SerializeField] private LeanGameObjectPool _autoPool;
    [SerializeField] private Vector2 _autoSpawnOffset;

    [Header("보너스 코인")]
    [SerializeField] private GameObject _bonusCoinPrefab;
    [SerializeField] private Vector2 _bonusCoinSpawnOffset;

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

    public void ShowBonusCoin(Vector3 position, int amount)
    {
        Vector3 spawnPosition = position + (Vector3)_bonusCoinSpawnOffset;

        GameObject floaterObject = Instantiate(_bonusCoinPrefab, spawnPosition, Quaternion.identity);
        DamageFloater floater = floaterObject.GetComponent<DamageFloater>();

        floater.ShowBonusCoin(amount);
    }
}
