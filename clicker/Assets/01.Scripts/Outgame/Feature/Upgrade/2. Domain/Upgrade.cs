using System;

public sealed class Upgrade
{
    public EUpgradeEffect Effect { get; }
    public string Description { get; }
    public int Level { get; private set; }
    public int MaxLevel { get; }

    private readonly double _baseCost;
    private readonly double _costMultiplier;
    private readonly double _baseValue;
    private readonly double _valueMultiplier;

    public bool IsMaxLevel => MaxLevel > 0 && Level >= MaxLevel;
    public double Cost => _baseCost * Math.Pow(_costMultiplier, Level); // 비용 규칙
    public double Damage => _baseValue + Level + _valueMultiplier; // 데미지 규칙

    public Upgrade(EUpgradeEffect effect, string description, int maxLevel,
                   double baseCost, double costMultiplier,
                   double baseValue, double valueMultiplier,
                   int level = 0)
    {
        Effect = effect;
        Description = description;
        MaxLevel = maxLevel;
        Level = Math.Max(0, level);
        _baseCost = baseCost;
        _costMultiplier = costMultiplier;
        _baseValue = baseValue;
        _valueMultiplier = valueMultiplier;
    }

    public bool TryLevelUp()
    {
        if (IsMaxLevel) return false;
        Level++;
        return true;
    }
}
