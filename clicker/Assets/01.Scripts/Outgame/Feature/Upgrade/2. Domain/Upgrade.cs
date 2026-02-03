using System;

public sealed class Upgrade
{
    public EUpgradeEffect Effect { get; }
    public string Description { get; }

    // 런타임 데이터
    public int Level { get; private set; }
    public int MaxLevel { get; }

    private readonly double _baseCost;
    private readonly double _costMultiplier;
    private readonly double _baseValue;
    private readonly double _valueMultiplier;

    // 규칙
    public bool IsMaxLevel => MaxLevel > 0 && Level >= MaxLevel;
    public double Cost => _baseCost * Math.Pow(_costMultiplier, Level);
    public double Damage => _baseValue + Level * _valueMultiplier;

    public Upgrade(UpgradeStepData stepData, UpgradeSpecData specData, int level = 0)
    {

        if (!Enum.IsDefined(typeof(EUpgradeEffect), stepData.Effect)) throw new ArgumentException($"유효하지 않은 Effect 입니다. ({stepData.Effect})", nameof(stepData.Effect));
        if (string.IsNullOrWhiteSpace(stepData.Description)) throw new ArgumentException($"description은 비어있을 수 없습니다. {stepData.Description}");
        if (stepData.MaxLevel < 0)  throw new ArgumentException($"maxLevel은 0 이상이어야 합니다. {stepData.MaxLevel}");
        if (level < 0) throw new ArgumentException($"level은 0 이상이어야 합니다. {level}");
        if (specData.BaseCost <= 0) throw new ArgumentException($"baseCost는 0 이상이어야 합니다. {specData.BaseCost}");
        if (specData.CostMultiplier <= 0) throw new ArgumentException($"costMultiplier는 0 이상이어야 합니다. {specData.CostMultiplier}");
        if (stepData.BaseValue <= 0) throw new ArgumentException($"baseValue는 0 이상이어야 합니다. {stepData.BaseValue}");
        if (stepData.ValueMultiplier <= 0) throw new ArgumentException($"valueMultiplier는 0 이상이어야 합니다. {stepData.ValueMultiplier}");

        Effect = stepData.Effect;
        Description = stepData.Description;
        MaxLevel = stepData.MaxLevel;
        Level = Math.Max(0, level);
        _baseCost = specData.BaseCost;
        _costMultiplier = specData.CostMultiplier;
        _baseValue = stepData.BaseValue;
        _valueMultiplier = stepData.ValueMultiplier;
    }

    public bool CanLevelUp()
    {
        return !IsMaxLevel;
    }

    public bool TryLevelUp()
    {
        if (!CanLevelUp()) return false;

        Level++;
        return true;
    }
}
