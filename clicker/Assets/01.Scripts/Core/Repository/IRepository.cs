using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IRepository<T> where T : class
{
    UniTask Save(T data);
    UniTask<T> Load();
}
