using UnityEngine;
using static SoundManager;

public class PlanetPop : MonoBehaviour
{
    [Header("별 배경 이펙트")]
    [SerializeField] private ParticleSystem _starParticleEffect;
    [SerializeField] private PlanetPressure _planetHealth;

    [Header("별 아이콘 이펙트")]
    [SerializeField] private GameObject _starPrefab;
    [SerializeField] private int _starCount = 10;
    [SerializeField] private float _yPosition = 2f;
    [SerializeField] private float _minExplosionForce = 3f;
    [SerializeField] private float _maxExplosionForce = 7f;

    [Header("스모크 이펙트")]
    [SerializeField] private ParticleSystem _smokeParticleEffect;

    private void Awake()
    {
        _planetHealth = GetComponent<PlanetPressure>();
    }

    private void Start()
    {
        if (_starPrefab != null)
        {
            PoolManager.Instance.Preload(_starPrefab, _starCount);
        }
    }

    private void OnEnable()
    {
        _planetHealth.OnDepleted += PlayPlanetPopEffect;
    }

    private void OnDisable()
    {
        _planetHealth.OnDepleted -= PlayPlanetPopEffect;
    }

    private void PlayPlanetPopEffect()
    {
        if (_starParticleEffect != null)
        {
            _starParticleEffect.transform.position = transform.position;
            _starParticleEffect.Play();
        }

        if (_smokeParticleEffect != null)
        {
            _smokeParticleEffect.transform.position = transform.position;
            _smokeParticleEffect.Play();
        }

        int bonusCoin = StageManager.Instance.CurrentPlanetData.BonusCoin;
        CoinManager.Instance.GetCoin(bonusCoin);
        TextFloaterSpawner.Instance.ShowBonusCoin(transform.position, bonusCoin);
        SoundManager.Instance.PlaySFX(Sfx.COIN);

        SpawnStars();
    }

    private void SpawnStars()
    {
        if (_starPrefab == null) return;

        for (int i = 0; i < _starCount; i++)
        {
            GameObject star = EffectSpawner.Instance.Spawn(_starPrefab, transform.position);

            if (star.TryGetComponent(out Rigidbody2D rb))
            {
                rb.linearVelocity = Vector2.zero;
                float randomX = Random.Range(-0.5f, 0.5f);
                float randomForce = Random.Range(_minExplosionForce, _maxExplosionForce);
                Vector2 direction = new Vector2(randomX, _yPosition).normalized;
                rb.AddForce(direction * randomForce, ForceMode2D.Impulse);
            }

            EffectSpawner.Instance.Despawn(star);
        }
    }
}
