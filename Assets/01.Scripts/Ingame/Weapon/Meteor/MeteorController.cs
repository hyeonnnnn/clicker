using UnityEngine;

[RequireComponent(typeof(MeteorMove))]
[RequireComponent(typeof(MeteorAttack))]
public class MeteorController : WeaponBase
{
    private MeteorMove _move;
    private MeteorAttack _attack;

    public Vector2 GetDirection() => _move.Direction;

    public void SetDirection(Vector2 dir) => _move.SetDirection(dir);

    protected override void Init()
    {
        _move = GetComponent<MeteorMove>();
        _attack = GetComponent<MeteorAttack>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.contacts[0];

        _move.OnCollision(contact.normal);

        if (collision.collider.CompareTag("Planet"))
        {
            _attack.Attack(collision.collider, contact);
        }
    }
}
