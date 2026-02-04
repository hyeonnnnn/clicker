using System;
using Firebase.Firestore;

[Serializable]
[FirestoreData]
public class PlanetSaveData
{
    [FirestoreProperty]
    public int CurrentStage { get; set; } // 행성의 순서

    [FirestoreProperty]
    public double CurrentPressure { get; set; }  // 현재 행성의 압력

    public static PlanetSaveData Default => new PlanetSaveData
    {
        CurrentStage = 0,
        CurrentPressure = 0
    };
}
