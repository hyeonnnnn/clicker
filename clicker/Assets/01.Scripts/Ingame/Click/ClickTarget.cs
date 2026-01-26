using UnityEngine;

public class ClickTarget : MonoBehaviour, Clickable
{
    [SerializeField] private string _name;

    private PlanetHealth _planetHealth;

    private void Awake()
    {
        _planetHealth = GetComponent<PlanetHealth>();
    }

    public bool OnClick(ClickInfo clickInfo)
    {
        if ( _planetHealth != null)
        {
            _planetHealth.TakeDamage(70);
        }

        var feedbacks = GetComponentsInChildren<IFeedback>();
        foreach (var feedback in feedbacks)
        {
            feedback.Play(clickInfo);
        }

        return true;
    }
}