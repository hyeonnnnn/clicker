using Firebase.Firestore;
using System;

[Serializable]
[FirestoreData]
public class SpawnSaveData
{
    [FirestoreProperty]
    public float[] RocketTimes { get; set; }

    [FirestoreProperty]
    public float[] MeteorDirections { get; set; }

    public static SpawnSaveData Default => new SpawnSaveData
    {
        RocketTimes = Array.Empty<float>(),
        MeteorDirections = Array.Empty<float>()
    };
}
