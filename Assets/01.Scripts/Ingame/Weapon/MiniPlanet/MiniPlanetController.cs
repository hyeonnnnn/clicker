using UnityEngine;

[RequireComponent(typeof(MiniPlanetAttack))]
public class MiniPlanetController : WeaponBase
{
    private MiniPlanetAttack _attack;

    protected override void Init()
    {
        _attack = GetComponent<MiniPlanetAttack>();
    }

    public void Initialize(Vector3 localPosition, Quaternion localRotation)
    {
        _attack.Initialize(localPosition, localRotation);
    }

    public void Attack()
    {
        _attack.Attack();
    }
}
