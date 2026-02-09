using UnityEngine;
using static SoundManager;

public class SoundFeedback : MonoBehaviour, IFeedback
{
    public void Play(ClickInfo clickInfo)
    {
        if (clickInfo.Type == EClickType.Auto) return;

        SoundManager.Instance.PlaySFX(Sfx.CLICKPLANET);
    }
}
