using UnityEngine;

public class Clicker : MonoBehaviour
{
    public LayerMask ClickLayer;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Input.mousePosition;
            Click(mousePosition);
        }
    }

    private void Click(Vector2 mousePosition)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
        if (hit == true)
        {
            Clickable clickable = hit.collider.GetComponent<Clickable>();

            ClickInfo clickInfo = new ClickInfo
            {
                Type = EClickType.Manual,
                Damage = GetManualClickDamage(),
                Position = worldPosition,
            };

            clickable?.OnClick(clickInfo);
        }
    }

    // 클릭 최종 데미지 계산
    private double GetManualClickDamage()
    {
        double baseDamage = 1;

        var flatUpgrade = UpgradeManager.Instance.GetUpgrade(EUpgradeEffect.ClickPower);
        var percentUpgrade = UpgradeManager.Instance.GetUpgrade(EUpgradeEffect.ClickPercent);

        double flat = flatUpgrade?.Damage ?? 0;
        double percent = percentUpgrade?.Damage ?? 0;

        return (baseDamage + flat) * (1 + percent / 100.0);
    }
}