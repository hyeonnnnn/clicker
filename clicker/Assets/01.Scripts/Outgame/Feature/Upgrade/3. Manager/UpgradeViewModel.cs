public struct UpgradeItemViewData
{
    public string Name;
    public string Description;
    public string Level;
    public string Cost;
    public bool CanPurchase;
}

public class UpgradeViewModel
{
    private UpgradeManager Manager => UpgradeManager.Instance;

    public UpgradeItemViewData GetItemViewData(EUpgradeType type)
    {
        var group = Manager.GetGroup(type);
        if (group == null) return default;

        var upgrade = group.GetCurrentUpgrade();
        bool isMax = group.IsAllMaxLevel();

        return new UpgradeItemViewData
        {
            Name = group.Name,
            Level = $"Lv.{group.GetTotalLevel()}",
            Description = upgrade != null ? FormatDescription(upgrade) : (isMax ? "Max Level" : ""),
            Cost = isMax ? "MAX" : (upgrade?.Cost.ToFormattedString() ?? ""),
            CanPurchase = Manager.CanLevelUp(type)
        };
    }

    private string FormatDescription(Upgrade upgrade)
    {
        string sign = upgrade.Effect == EUpgradeEffect.RocketCooldown ? "-" : "+";
        return $"{upgrade.Description} {sign}{upgrade.BaseValue.ToFormattedString()}";
    }
}
