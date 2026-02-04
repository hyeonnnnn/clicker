using UnityEngine;
using static EffectSpawner;

public class EffectFeedback : MonoBehaviour, IFeedback
{
    public void Play(ClickInfo clickInfo)
    {
        if (clickInfo.Type == EClickType.Auto) return;

        Vector2 position = clickInfo.Position;

        float randomZ = Random.Range(0f, 360f);
        Quaternion rotation = Quaternion.Euler(0f, 0f, randomZ);

        EffectSpawner.Instance.PlayEffect(Effect.CLICKPLANET, position, rotation);
    }
}
