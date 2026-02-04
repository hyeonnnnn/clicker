using System;

public class Planet
{
    public int Level { get; private set; }
    public double CurrentPressure { get; private set; }

    private readonly double _basePressure;
    private readonly double _pressureMultiplier;
    private readonly double _baseBonusCoin;
    private readonly double _bonusCoinMultiplier;

    // 규칙
    public double MaxPressure => _basePressure * Math.Pow(_pressureMultiplier, Level);
    public double BonusCoin => _baseBonusCoin * Math.Pow(_bonusCoinMultiplier, Level);

    public Planet(PlanetSpecData specData, int level = 0, double currentPressure = 0)
    {
        if (specData.BasePressure <= 0) throw new ArgumentException($"BasePressure는 0보다 커야 합니다. {specData.BasePressure}");
        if (specData.PressureMultiplier <= 0) throw new ArgumentException($"PressureMultiplier는 0보다 커야 합니다. {specData.PressureMultiplier}");
        if (specData.BaseBonusCoin <= 0) throw new ArgumentException($"BaseBonusCoin은 0보다 커야 합니다. {specData.BaseBonusCoin}");
        if (specData.BonusCoinMultiplier <= 0) throw new ArgumentException($"BonusCoinMultiplier는 0보다 커야 합니다. {specData.BonusCoinMultiplier}");

        _basePressure = specData.BasePressure;
        _pressureMultiplier = specData.PressureMultiplier;
        _baseBonusCoin = specData.BaseBonusCoin;
        _bonusCoinMultiplier = specData.BonusCoinMultiplier;
        Level = Math.Max(0, level);
        CurrentPressure = Math.Max(0, currentPressure);
    }

    public void UpdatePressure(double pressure)
    {
        CurrentPressure = Math.Clamp(pressure, 0, MaxPressure);
    }

    public void NextLevel()
    {
        Level++;
        CurrentPressure = 0;
    }
}
