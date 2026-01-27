using Lean.Pool;
using UnityEngine;

public class DamageFloaterSpawner : MonoBehaviour
{
    public static DamageFloaterSpawner Instance { get; private set; }

    [SerializeField] private LeanGameObjectPool _pool;

    private void Awake()
    {
        Instance = this;
        _pool = GetComponent<LeanGameObjectPool>();
    }

    public void ShowDamage(ClickInfo clickInfo)
    {
        // 풀로부터 데미지 플로터를 가져와서
        GameObject damageFloaterObject = _pool.Spawn(clickInfo.Position, Quaternion.identity);
        DamageFloater damageFloater = damageFloaterObject.GetComponent<DamageFloater>();

        // 클릭한 위치에 생성한다.
        damageFloater.Show(clickInfo);
    }
}
