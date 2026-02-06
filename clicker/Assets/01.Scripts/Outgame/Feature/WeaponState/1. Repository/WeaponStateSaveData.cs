using Firebase.Firestore;
using System;

[Serializable]
[FirestoreData]
public class WeaponStateSaveData
{
    [FirestoreProperty]
    public float[] RocketTimes { get; set; }

    [FirestoreProperty]
    public int MeteorCount { get; set; }

    public static WeaponStateSaveData Default => new WeaponStateSaveData
    {
        RocketTimes = Array.Empty<float>(),
        MeteorCount = 0
    };
}
