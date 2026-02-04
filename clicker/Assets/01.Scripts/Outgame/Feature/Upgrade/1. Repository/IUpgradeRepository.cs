using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IUpgradeRepository
{
    UniTask Save(UpgradeSaveData data);
    UniTask<UpgradeSaveData> Load();
}
