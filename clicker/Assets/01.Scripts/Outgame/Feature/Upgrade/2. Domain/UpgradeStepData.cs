using System;

[Serializable]
public class UpgradeStepData
{
    public string Description;
    public EUpgradeEffect Effect;
    public double BaseValue;
    public double ValueMultiplier;
    public int MaxLevel;
}

public enum EUpgradeEffect
{
    // ManualClick
    ClickPower,
    ClickPercent,

    // Rocket
    RocketCount,
    RocketCooldown,
    RocketPower,

    // Rock
    RockCount,
    RockSpeed,
    RockPower,

    Count
}
