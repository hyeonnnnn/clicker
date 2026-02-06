using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpgradeItem : MonoBehaviour
{
    [Header("데이터")]
    [SerializeField] private EUpgradeType _upgradeType;

    [Header("UI 컴포넌트")]
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private Button _purchaseButton;

    private readonly UpgradeViewModel _viewModel = new();

    private void Start()
    {
        _purchaseButton.onClick.AddListener(OnPurchaseClicked);
        Refresh();
    }

    public void Refresh()
    {
        var data = _viewModel.GetItemViewData(_upgradeType);

        _nameText.text = data.Name;
        _descriptionText.text = data.Description;
        _levelText.text = data.Level;
        _costText.text = data.Cost;
        _purchaseButton.interactable = data.CanPurchase;
    }

    private void OnPurchaseClicked()
    {
        // 중복 클릭 방지
        _purchaseButton.interactable = false;
        UpgradeManager.Instance.TryLevelUp(_upgradeType);
        Refresh();
    }
}
