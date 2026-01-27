using UnityEngine;

public class PlanetCrack : MonoBehaviour
{
    [SerializeField] private GameObject[] _cracks;
    [SerializeField] private PlanetPressure _planetHealth;
    [SerializeField] private float[] _crackThresholds = { 0.8f, 0.5f, 0.1f };

    private void Awake()
    {
        _planetHealth = GetComponent<PlanetPressure>();
    }

    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        _planetHealth.OnPressureChanged += ShowCrack;
        _planetHealth.OnDepleted += Initialize;
    }

    private void OnDisable()
    {
        _planetHealth.OnPressureChanged -= ShowCrack;
        _planetHealth.OnDepleted -= Initialize;
    }

    private void Initialize()
    {
        foreach (var crack in _cracks)
        {
            crack.SetActive(false);
        }
    }

    private void ShowCrack(int currentHealth, int maxHealth)
    {
        float healthPercent = (float)currentHealth / maxHealth;

        for (int i = 0; i < _cracks.Length && i < _crackThresholds.Length; i++)
        {
            if (healthPercent <= _crackThresholds[i])
            {
                _cracks[i].SetActive(true);
            }
        }
    }
}
