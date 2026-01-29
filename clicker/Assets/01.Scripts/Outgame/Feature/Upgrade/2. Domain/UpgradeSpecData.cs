using System;

[Serializable]
public class UpgradeSpecData
{
    public EUpgradeType Type;
    public string Name;
    public double BaseCost;
    public double CostMultiplier;
    public UpgradeStepData[] Steps; // 레벨에 따라 순환
}
