using System;
using System.Collections.Generic;

// (클릭/로켓/운석)을 대표하는 도메인 객체
public sealed class UpgradeGroup
{
    public EUpgradeType Type { get; }
    public string Name { get; }
    public int Cursor => _cursor;

    private readonly EUpgradeEffect[] _effects;
    private int _cursor; // 어떤 걸 보여줄 지 -> 위치
    private readonly IReadOnlyDictionary<EUpgradeEffect, Upgrade> _upgrades;

    public UpgradeGroup(EUpgradeType type, string name, EUpgradeEffect[] effects,
                        IReadOnlyDictionary<EUpgradeEffect, Upgrade> upgrades,
                        int initialCursor = 0)
    {
        Type = type;
        Name = name;
        _effects = effects;
        _upgrades = upgrades;
        _cursor = Math.Max(0, Math.Min(initialCursor, _effects.Length - 1));
    }


    public Upgrade GetCurrentUpgrade()
    {
        var effect = GetCurrentEffect();
        return effect.HasValue ? _upgrades[effect.Value] : null;
    }

    // 각 이펙트에 대해서 Max가 아닌 첫 효과를 찾으면
    // -> 현재 표시 효과로
    public EUpgradeEffect? GetCurrentEffect()
    {
        if (_effects.Length == 0) return null;

        for (int i = 0; i < _effects.Length; i++)
        {
            int idx = (_cursor + i) % _effects.Length;
            var effect = _effects[idx];

            if (!_upgrades[effect].IsMaxLevel)
            {
                _cursor = idx;
                return effect;
            }
        }
        return null;
    }

    // 다음 표시로 넘기기
    public EUpgradeEffect? AdvanceToNextAvailable()
    {
        if (_effects.Length == 0) return null;

        for (int i = 1; i <= _effects.Length; i++)
        {
            int idx = (_cursor + i) % _effects.Length;
            var effect = _effects[idx];

            if (!_upgrades[effect].IsMaxLevel)
            {
                _cursor = idx;
                return effect;
            }
        }
        return null;
    }

    public bool IsAllMaxLevel()
    {
        foreach (var effect in _effects)
        {
            if (!_upgrades[effect].IsMaxLevel) return false;
        }
        return true;
    }

    public int GetTotalLevel()
    {
        int total = 0;
        foreach (var effect in _effects)
        {
            total += _upgrades[effect].Level;
        }
        return total;
    }
}
