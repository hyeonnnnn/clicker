using UnityEngine;

public class DamageFloaterFeedback : MonoBehaviour, IFeedback
{
    public void Play(ClickInfo clickInfo)
    {
        TextFloaterSpawner.Instance.ShowDamage(clickInfo);
    }
}
