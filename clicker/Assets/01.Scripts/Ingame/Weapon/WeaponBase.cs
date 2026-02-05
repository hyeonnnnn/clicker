using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public bool IsActive { get; private set; } = true;

    protected virtual void Awake()
    {
        Init();
    }

    protected abstract void Init();

    public virtual void Activate()
    {
        IsActive = true;
        gameObject.SetActive(true);
    }

    public virtual void Deactivate()
    {
        IsActive = false;
        gameObject.SetActive(false);
    }
}
