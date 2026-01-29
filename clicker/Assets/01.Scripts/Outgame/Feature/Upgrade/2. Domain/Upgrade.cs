using System;

public class Upgrade
{
    public readonly UpgradeSpecData SpecData; 
    public readonly UpgradeStepData StepData; 

    public int Level { get; private set; }

    public Currency Cost => SpecData.BaseCost + Math.Pow(SpecData.CostMultiplier, Level); // cost 규칙
    public double Damage => StepData.BaseValue + Level + StepData.ValueMultiplier; // value 규칙
    public bool IsMaxLevel => Level >= StepData.MaxLevel && StepData.MaxLevel > 0;

    public Upgrade(UpgradeSpecData specData, UpgradeStepData stepData)
    {
        SpecData = specData;
        StepData = stepData;

        // specData
        if (specData.BaseCost <= 0) throw new ArgumentOutOfRangeException($"기본 비용은 0보다 커야합니다. {specData.BaseCost}");
        if (specData.CostMultiplier <= 0) throw new ArgumentOutOfRangeException($"비용 증가량은 0보다 커야합니다. {specData.CostMultiplier}");
        if (specData.Steps == null || specData.Steps.Length == 0) throw new ArgumentOutOfRangeException($"Steps가 비어있을 수 없습니다.");

        // stepData
        if (string.IsNullOrEmpty(stepData.Description)) throw new ArgumentOutOfRangeException($"설명 텍스트가 비어있을 수 없습니다.");
        if (stepData.MaxLevel < 0) throw new ArgumentOutOfRangeException($"최대 레벨은 0보다 커야합니다. {StepData.MaxLevel}");
    }


    public bool TryLevelUp()
    {
        // Todo. 레벨업 검사

        Level++;
        return true;
    }
}

