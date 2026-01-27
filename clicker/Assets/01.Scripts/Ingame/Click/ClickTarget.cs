using UnityEngine;
using static SoundManager;

public class ClickTarget : MonoBehaviour, Clickable
{
    [SerializeField] private string _name;

    private PlanetPressure _planetHealth;

    private void Awake()
    {
        _planetHealth = GetComponent<PlanetPressure>();
    }

    public bool OnClick(ClickInfo clickInfo)
    {
        if ( _planetHealth != null)
        {
            _planetHealth.TakeDamage((int)clickInfo.Damage);
        }

        var feedbacks = GetComponentsInChildren<IFeedback>();
        foreach (var feedback in feedbacks)
        {
            feedback.Play(clickInfo);
        };

        return true;
    }
}