using DG.Tweening;
using UnityEngine;

public class PlanetExpansion : MonoBehaviour
{
    [Header("대상")]
    [SerializeField] private ClickTarget _owner;

    [Header("팽창 설정")]
    [SerializeField] private float _maxScaleMultiplier = 1.7f;
    [SerializeField] private float _lerpSpeed = 10f; 

    private Vector3 _originalScale;
    private Vector3 _targetScale;

    private void Awake()
    {
        if (_owner == null) _owner = GetComponent<ClickTarget>();
        _originalScale = transform.localScale;
        _targetScale = _originalScale;
    }

    public void Initialize()
    {
        transform.localScale = _originalScale;
        _targetScale = _originalScale;
    }

    public void ExpendPlanet(int currentPressure, int maxPressure)
    {
        float ratio = (float)currentPressure / maxPressure;
        float scaleOffset = (_maxScaleMultiplier - 1f) * ratio;
        _targetScale = _originalScale * (1f + scaleOffset);

        _owner.transform.DOScale(_targetScale, 0.2f).SetEase(Ease.OutCubic);
    }
}