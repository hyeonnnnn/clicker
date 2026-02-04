using UnityEngine;
using static EffectSpawner;
using static SoundManager;

public static class WeaponFeedback
{
    public static void PlayImpact(ClickInfo info, Effect effect, Sfx sfx, Vector2 position, Quaternion rotation)
    {
        TextFloaterSpawner.Instance.ShowDamage(info);
        EffectSpawner.Instance.PlayEffect(effect, position, rotation);
        SoundManager.Instance.PlaySFX(sfx);
    }
}
