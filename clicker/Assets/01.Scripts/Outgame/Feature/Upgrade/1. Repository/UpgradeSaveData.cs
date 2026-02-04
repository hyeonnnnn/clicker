using System;
using Firebase.Firestore;

[Serializable]
[FirestoreData]
public class UpgradeSaveData
{
    [FirestoreProperty]
    public int[] EffectLevels { get; set; }

    [FirestoreProperty]
    public int[] TypeCursors { get; set; }

    [FirestoreProperty]
    public string LastSaveTime { get; set; }

    public static UpgradeSaveData Default => new UpgradeSaveData
    {
        EffectLevels = new int[(int)EUpgradeEffect.Count],
        TypeCursors = new int[(int)EUpgradeType.Count],
        LastSaveTime = DateTime.Now.ToString("o")
    };
}
