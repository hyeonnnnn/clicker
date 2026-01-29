using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpgradeItem : MonoBehaviour
{
    [SerializeField] private EUpgradeEffect[] _effects;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private Button _purchaseButton;

    private int _currentIndex;
    private Upgrade _upgrade;

    private EUpgradeEffect CurrentEffect => _effects[_currentIndex];

    private void Start()
    {
        _purchaseButton.onClick.AddListener(OnPurchaseClicked);
    }

    public void Refresh()
    {
        if (_effects == null || _effects.Length == 0) return;

        FindNextAvailable();

        _upgrade = UpgradeManager.Instance.Get(CurrentEffect);
        if (_upgrade == null) return;

        _nameText.text = _upgrade.SpecData.Name;
        _descriptionText.text = _upgrade.StepData.Description;
        _levelText.text = $"Lv.{_upgrade.Level}";

        if (IsAllMaxLevel())
        {
            _costText.text = "MAX";
            _purchaseButton.interactable = false;
        }
        else
        {
            _costText.text = _upgrade.Cost.ToString();
            _purchaseButton.interactable = UpgradeManager.Instance.CanLevelUp(CurrentEffect);
        }
    }

    private void OnPurchaseClicked()
    {
        if (_effects == null || _effects.Length == 0) return;

        if (UpgradeManager.Instance.TryUpgrade(CurrentEffect))
        {
            MoveToNext();
        }
    }

    private void MoveToNext()
    {
        _currentIndex = (_currentIndex + 1) % _effects.Length;
        FindNextAvailable();
    }

    private void FindNextAvailable()
    {
        if (IsAllMaxLevel()) return;

        for (int i = 0; i < _effects.Length; i++)
        {
            var upgrade = UpgradeManager.Instance.Get(CurrentEffect);
            if (upgrade != null && !upgrade.IsMaxLevel)
            {
                return;
            }
            _currentIndex = (_currentIndex + 1) % _effects.Length;
        }
    }

    private bool IsAllMaxLevel()
    {
        foreach (var effect in _effects)
        {
            var upgrade = UpgradeManager.Instance.Get(effect);
            if (upgrade != null && !upgrade.IsMaxLevel)
            {
                return false;
            }
        }
        return true;
    }
}
