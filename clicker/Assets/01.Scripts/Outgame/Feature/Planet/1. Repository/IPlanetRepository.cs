using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IPlanetRepository
{
    UniTask Save(PlanetSaveData data);
    UniTask<PlanetSaveData> Load();
}
