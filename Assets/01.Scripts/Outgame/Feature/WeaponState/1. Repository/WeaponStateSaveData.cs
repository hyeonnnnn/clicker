#if !UNITY_WEBGL || UNITY_EDITOR
using Firebase.Firestore;
#endif
using System;

[Serializable]
[FirestoreData]
public class WeaponStateSaveData : ISaveData
{
    [FirestoreProperty]
    public float[] RocketTimes { get; set; }

    [FirestoreProperty]
    public int MeteorCount { get; set; }

    [FirestoreProperty]
    public string LastSavedAt { get; set; }

    public static WeaponStateSaveData Default => new WeaponStateSaveData
    {
        RocketTimes = Array.Empty<float>(),
        MeteorCount = 0,
        LastSavedAt = null
    };
}
