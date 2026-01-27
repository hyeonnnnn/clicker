using UnityEngine;
using TMPro;
using Lean.Pool;
using DG.Tweening;

public class DamageFloater : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;

    [Header("Animation")]
    [SerializeField] private float _duration = 0.8f;
    [SerializeField] private float _floatHeight = 1f;
    [SerializeField] private float _randomOffsetX = 0.3f;

    [Header("Scale")]
    [SerializeField] private float _punchScale = 0.3f;
    [SerializeField] private float _punchDuration = 0.2f;

    private Vector3 _initialScale;

    private void Awake()
    {
        _initialScale = transform.localScale;
    }

    public void Show(ClickInfo clickInfo)
    {
        _text.text = clickInfo.Damage.ToFormattedString();
        _text.alpha = 1f;
        transform.localScale = _initialScale;

        float randomX = Random.Range(-_randomOffsetX, _randomOffsetX);
        Vector3 targetPosition = transform.position + new Vector3(randomX, _floatHeight, 0f);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOMove(targetPosition, _duration).SetEase(Ease.OutCubic));
        sequence.Join(_text.DOFade(0f, _duration).SetEase(Ease.InQuad));
        sequence.Join(transform.DOPunchScale(Vector3.one * _punchScale, _punchDuration, 1, 0f));

        sequence.OnComplete(() => LeanPool.Despawn(gameObject));
    }

    private void OnDisable()
    {
        DOTween.Kill(transform);
        DOTween.Kill(_text);
    }
}
