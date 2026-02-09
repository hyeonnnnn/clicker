using System;
#if !UNITY_WEBGL || UNITY_EDITOR
using Firebase.Firestore;
#endif

[Serializable]
[FirestoreData]
public class UpgradeSaveData : ISaveData
{
    [FirestoreProperty]
    public int[] EffectLevels { get; set; }

    [FirestoreProperty]
    public int[] TypeCursors { get; set; }

    [FirestoreProperty]
    public string LastSavedAt { get; set; }

    public static UpgradeSaveData Default => new UpgradeSaveData
    {
        EffectLevels = new int[(int)EUpgradeEffect.Count],
        TypeCursors = new int[(int)EUpgradeType.Count],
        LastSavedAt = DateTime.Now.ToString("o")
    };
}
