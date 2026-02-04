using Cysharp.Threading.Tasks;
using UnityEngine;

public class ClickTarget : MonoBehaviour, IClickable
{
    [SerializeField] private string _name;

    private PlanetPressure _planetHealth;
    private IFeedback[] _feedbacks;

    private void Awake()
    {
        _planetHealth = GetComponent<PlanetPressure>();
        _feedbacks = GetComponentsInChildren<IFeedback>();
    }

    private void OnEnable()
    {
        if (_planetHealth != null)
        {
            _planetHealth.OnDamaged += OnPlanetDamaged;
        }
    }

    private void OnDisable()
    {
        if (_planetHealth != null)
        {
            _planetHealth.OnDamaged -= OnPlanetDamaged;
        }
    }

    public bool OnClick(ClickInfo clickInfo)
    {
        if ( _planetHealth != null)
        {
            _planetHealth.TakeDamage(clickInfo.Damage);
        }

        if (_feedbacks != null)
        {
            for (int i = 0; i < _feedbacks.Length; i++)
            {
                _feedbacks[i].Play(clickInfo);
            }
        }

        return true;
    }

    private void OnPlanetDamaged(double damage)
    {
        CurrencyManager.Instance.Add(ECurrencyType.Star, damage).Forget();
    }
}
