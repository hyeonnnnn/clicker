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
        if (hit.collider != null)
        {
            double finalDamage = UpgradeManager.Instance.GetUpgrade(EUpgradeEffect.ClickPower)?.Value ?? 0;
            IClickable clickable = hit.collider.GetComponent<IClickable>();

            if (clickable != null)
            {
                ClickInfo clickInfo = new ClickInfo
                {
                    Type = EClickType.Manual,
                    Damage = finalDamage,
                    Position = worldPosition,
                };

                clickable.OnClick(clickInfo);
            }
        }
    }
}