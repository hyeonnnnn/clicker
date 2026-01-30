using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpgradeItem : MonoBehaviour
{
    [SerializeField] private EUpgradeType _upgradeType;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private Button _purchaseButton;

    private void Start()
    {
        _purchaseButton.onClick.AddListener(OnPurchaseClicked);
    }

    public void Refresh()
    {
        // 그룹을 가져와서
        var group = UpgradeManager.Instance.GetGroup(_upgradeType);
        if (group == null) return;

        var upgrade = group.GetCurrentUpgrade();

        // ui 텍스트 채우기
        _nameText.text = group.Name;
        _descriptionText.text = upgrade != null ? upgrade.Description : "";
        _levelText.text = $"Lv.{group.GetTotalLevel()}";

        if (group.IsAllMaxLevel())
        {
            _costText.text = "MAX";
            _purchaseButton.interactable = false;
        }
        else
        {
            _costText.text = upgrade != null ? upgrade.Cost.ToString() : "";
            _purchaseButton.interactable = UpgradeManager.Instance.CanUpgradeType(_upgradeType);
        }
    }

    private void OnPurchaseClicked()
    {
        UpgradeManager.Instance.TryUpgradeType(_upgradeType);
    }
}
