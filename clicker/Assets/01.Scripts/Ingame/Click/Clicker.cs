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
                Damage = GameManager.Instance.ManualDamage,
                Position = worldPosition,
            };

            clickable?.OnClick(clickInfo);
        }
    }
}