using System;

public class Upgrade
{
    public readonly UpgradeSpecData SpecData;

    public int Level {  get; private set; }

    public Currency Cost => SpecData.BaseCost + Math.Pow(SpecData.CostMultiplier, Level); // 지수 공식
    public double Damage => SpecData.BaseDamage + Level * SpecData.DamageMultiplier; // 선형 공식
    public bool IsMaxLevel => SpecData.MaxLevel > 0 && Level >= SpecData.MaxLevel;

    // 핵심 규칙을 작성한다.
    public Upgrade(UpgradeSpecData specData)
    {
        SpecData = specData;

        if (specData.MaxLevel < 0) throw new System.ArgumentOutOfRangeException($"최대 레벨은 0보다 커야합니다. {specData.MaxLevel}");
        if (specData.BaseCost <= 0) throw new System.ArgumentOutOfRangeException($"기본 비용은 0보다 커야합니다. {specData.BaseCost}");
        if (specData.BaseDamage <= 0) throw new System.ArgumentOutOfRangeException($"기본 데미지은 0보다 커야합니다. {specData.BaseDamage}");
        if (specData.CostMultiplier <= 0) throw new System.ArgumentOutOfRangeException($"비용 증가량은 0보다 커야합니다. {specData.CostMultiplier}");
        if (specData.DamageMultiplier <= 0) throw new System.ArgumentOutOfRangeException($"데미지 증가량은 0보다 커야합니다. {specData.DamageMultiplier}");
        if (string.IsNullOrEmpty(specData.Name)) throw new System.ArgumentOutOfRangeException($"이름은 비어있을 수 없습니다.");
        if (string.IsNullOrEmpty(specData.Description)) throw new System.ArgumentOutOfRangeException($"설명은 비어있을 수 없습니다.");
    }

    public bool TryLevelUp()
    {
        if (IsMaxLevel) return false;

        Level++;
        return true;
    }
}
