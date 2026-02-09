using DG.Tweening;
using UnityEngine;

public class ScaleExpansionFeedback : MonoBehaviour, IFeedback
{
    [Header("대상")]
    [SerializeField] private ClickTarget _owner;

    [Header("팽창 설정")]
    [SerializeField] private float _maxScaleMultiplier = 1.7f;

    [Header("애니메이션 설정")]
    [SerializeField] private float _punchScale = 0.2f;
    [SerializeField] private float _duration = 0.15f;

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

    public void Play(ClickInfo clickInfo)
    {
        ExpandPlanet(clickInfo.CurrentPressure, clickInfo.MaxPressure);
    }

    public void ExpandPlanet(double currentPressure, double maxPressure)
    {
        if (maxPressure <= 0) return;

        double ratio = currentPressure / maxPressure;
        double scaleOffset = (_maxScaleMultiplier - 1f) * ratio;
        _targetScale = _originalScale * (1f + (float)scaleOffset);

        _owner.transform.DOKill();
        Sequence seq = DOTween.Sequence();
        seq.Append(_owner.transform.DOScale(_targetScale * (1f + _punchScale), _duration * 0.5f).SetEase(Ease.OutQuad));
        seq.Append(_owner.transform.DOScale(_targetScale, _duration * 0.5f).SetEase(Ease.InQuad));
    }
}