using Cysharp.Threading.Tasks;

public interface ISpawnRepository
{
    UniTask Save(SpawnSaveData saveData);
    UniTask<SpawnSaveData> Load();
}
