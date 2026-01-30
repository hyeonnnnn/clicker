using System;

[Serializable]
public struct UpgradeSaveData
{
    public int[] EffectLevels;
    public int[] TypeCursors;
    public string LastSaveTime;

    public static UpgradeSaveData Default => new UpgradeSaveData
    {
        EffectLevels = new int[(int)EUpgradeEffect.Count],
        TypeCursors = new int[(int)EUpgradeType.Count],
        LastSaveTime = DateTime.Now.ToString("o")
    };
}
