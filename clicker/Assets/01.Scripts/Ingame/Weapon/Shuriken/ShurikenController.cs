using UnityEngine;

[RequireComponent(typeof(ShurikenMove))]
public class ShurikenController : MonoBehaviour
{
    private ShurikenMove _move;

    private void Awake()
    {
        _move = GetComponent<ShurikenMove>();
    }

    private void Update()
    {
        _move.Rotation();
    }
}
