using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlanetInfo", menuName = "Clicker/PlanetInfo")]
public class PlanetInfo : ScriptableObject
{
    private const int PlanetCount = 10;

    [SerializeField] private PlanetData[] _planets = new PlanetData[PlanetCount];

    public PlanetData GetPlanet(int index) => _planets[index];
    public int Count => _planets.Length;
}

[Serializable]
public class PlanetData
{
    public Sprite Sprite;
    public Sprite Icon;
    public int Health;
    public int BonusCoin;
    public string Name;
    public int Number;
}
