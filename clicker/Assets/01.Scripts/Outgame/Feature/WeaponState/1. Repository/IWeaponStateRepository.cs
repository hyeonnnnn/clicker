using Cysharp.Threading.Tasks;

public interface IWeaponStateRepository
{
    UniTask Save(WeaponStateSaveData saveData);
    UniTask<WeaponStateSaveData> Load();
}
