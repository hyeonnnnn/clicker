using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IRepository<T>
{
    UniTask Save(T data);
    UniTask<T> Load();
}
