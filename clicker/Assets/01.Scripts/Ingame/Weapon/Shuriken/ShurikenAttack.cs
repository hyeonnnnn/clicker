using UnityEngine;

public class ShurikenAttack : MonoBehaviour
{
    [SerializeField] private PlanetHealth _planetHealth;
    [SerializeField] private int _damage = 10;

    public void Attack()
    {
        Debug.Log("공격");
        _planetHealth.TakeDamage(_damage);
    }
}
