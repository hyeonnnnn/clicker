using System;
using Firebase.Firestore;

[Serializable]
[FirestoreData]
public class PlanetSaveData : ISaveData
{
    [FirestoreProperty]
    public int CurrentStage { get; set; }

    [FirestoreProperty]
    public double CurrentPressure { get; set; } 

    [FirestoreProperty]
    public string LastSavedAt { get; set; }

    public static PlanetSaveData Default => new PlanetSaveData
    {
        CurrentStage = 0,
        CurrentPressure = 0,
        LastSavedAt = null
    };
}
