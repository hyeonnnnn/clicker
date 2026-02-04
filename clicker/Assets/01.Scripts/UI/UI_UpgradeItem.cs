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

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _purchaseButton.onClick.AddListener(() => OnPurchaseClicked().Forget());
        Refresh();
    }

    public void Refresh()
    {
        var data = UpgradeManager.Instance.GetUpgradeItemViewData(_upgradeType);

        _nameText.text = data.Name;
        _descriptionText.text = data.Description;
        _levelText.text = data.Level;
        _costText.text = data.Cost;
        _purchaseButton.interactable = data.CanPurchase;
    }

    private async UniTask OnPurchaseClicked()
    {
        // 중복 클릭 방지
        _purchaseButton.interactable = false;
        bool success = await UpgradeManager.Instance.TryLevelUp(_upgradeType);
        Refresh();
    }
}
