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
    public double Damage => _baseValue + Level * _valueMultiplier; // 데미지 규칙

    public Upgrade(EUpgradeEffect effect, string description, int maxLevel,
                   double baseCost, double costMultiplier,
                   double baseValue, double valueMultiplier,
                   int level = 0)
    {

        if (!Enum.IsDefined(typeof(EUpgradeEffect), effect)) throw new ArgumentException($"유효하지 않은 Effect 입니다. ({effect})", nameof(effect));
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("description은 비어있을 수 없습니다.", nameof(description));
        if (maxLevel < 0)  throw new ArgumentException("maxLevel은 0 이상이어야 합니다.", nameof(maxLevel));
        if (level < 0) throw new ArgumentException("level은 0 이상이어야 합니다.", nameof(level));
        if (baseCost <= 0) throw new ArgumentException("baseCost는 0 이상이어야 합니다.", nameof(baseCost));
        if (costMultiplier <= 0) throw new ArgumentException("costMultiplier는 0 이상이어야 합니다.", nameof(costMultiplier));
        if (baseValue <= 0) throw new ArgumentException("baseValue는 0 이상이어야 합니다.", nameof(baseValue));
        if (valueMultiplier <= 0) throw new ArgumentException("valueMultiplier는 0 이상이어야 합니다.", nameof(valueMultiplier));

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
