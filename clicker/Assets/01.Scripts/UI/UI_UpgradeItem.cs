using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpgradeItem : MonoBehaviour
{
    [SerializeField] private EUpgradeType[] _upgradeTypes;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private Button _purchaseButton;

    private int _currentIndex;
    private Upgrade _upgrade;

    private EUpgradeType CurrentType => _upgradeTypes[_currentIndex];

    private void Start()
    {
        _purchaseButton.onClick.AddListener(OnPurchaseClicked);
    }

    public void Refresh()
    {
        if (_upgradeTypes == null || _upgradeTypes.Length == 0) return;

        _upgrade = UpgradeManager.Instance.Get(CurrentType);
        if (_upgrade == null) return;

        _nameText.text = _upgrade.SpecData.Name;
        _descriptionText.text = _upgrade.SpecData.Description;
        _levelText.text = $"Lv.{_upgrade.Level}";

        if (_upgrade.IsMaxLevel)
        {
            _costText.text = "MAX";
            _purchaseButton.interactable = false;
        }
        else
        {
            _costText.text = _upgrade.Cost.ToString();
            _purchaseButton.interactable = UpgradeManager.Instance.CanAfford(CurrentType);
        }
    }

    private void OnPurchaseClicked()
    {
        if (UpgradeManager.Instance.TryPurchase(CurrentType))
        {
            _currentIndex = (_currentIndex + 1) % _upgradeTypes.Length;
        }
    }
}
