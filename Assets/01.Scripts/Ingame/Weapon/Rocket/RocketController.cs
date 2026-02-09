using System;
using UnityEngine;

[RequireComponent(typeof(RocketMove))]
[RequireComponent(typeof(RocketAttack))]
public class RocketController : WeaponBase
{
    private RocketMove _move;
    private RocketAttack _attack;

    public event Action OnPassedCenter;

    public float GetTime() => _move.CurrentTime;

    public void SetTimeOffset(float offset)
    {
        _move.SetTimeOffset(offset);
    }

    protected override void Init()
    {
        _move = GetComponent<RocketMove>();
        _attack = GetComponent<RocketAttack>();
    }

    private void Start()
    {
        var planet = GameObject.FindGameObjectWithTag("Planet");
        if (planet != null)
        {
            var target = planet.transform;
            var planetPressure = planet.GetComponent<PlanetPressure>();

            _move.Initialize(target);
            _attack.Initialize(target, planetPressure);
        }

        _move.OnPassedCenter += HandlePassedCenter;
    }

    private void OnDestroy()
    {
        _move.OnPassedCenter -= HandlePassedCenter;
    }

    private void HandlePassedCenter()
    {
        _attack.Attack();
        OnPassedCenter?.Invoke();
    }
}
