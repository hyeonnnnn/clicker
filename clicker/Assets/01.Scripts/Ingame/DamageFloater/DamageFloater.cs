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
    private Material _material;
    private float _initialGlowPower;

    private static readonly int GlowPower = Shader.PropertyToID("_GlowPower");

    private void Awake()
    {
        _initialScale = transform.localScale;
        _material = _text.fontMaterial;
        _initialGlowPower = _material.GetFloat(GlowPower);

        var renderer = GetComponent<MeshRenderer>();
        renderer.sortingLayerName = "Effect";
        renderer.sortingOrder = 1;
    }

    public void ShowManual(ClickInfo clickInfo)
    {
        _text.text = "+" + clickInfo.Damage.ToFormattedString();
        _text.alpha = 1f;
        _material.SetFloat(GlowPower, _initialGlowPower);
        transform.localScale = _initialScale;

        float randomX = Random.Range(-_randomOffsetX, _randomOffsetX);
        Vector3 targetPosition = transform.position + new Vector3(randomX, _floatHeight, 0f);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOMove(targetPosition, _duration).SetEase(Ease.OutCubic));
        sequence.Join(_text.DOFade(0f, _duration).SetEase(Ease.InQuad));
        sequence.Join(_material.DOFloat(0f, GlowPower, _duration).SetEase(Ease.InQuad));
        sequence.Join(transform.DOPunchScale(Vector3.one * _punchScale, _punchDuration, 1, 0f));

        sequence.OnComplete(() => LeanPool.Despawn(gameObject));
    }

    public void ShowAuto(ClickInfo clickInfo)
    {
        _text.text = "+" + clickInfo.Damage.ToFormattedString();
        _text.alpha = 1f;
        _material.SetFloat(GlowPower, _initialGlowPower);
        transform.localScale = Vector3.zero;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOScale(_initialScale * 1.2f, _duration * 0.3f).SetEase(Ease.OutBack));
        sequence.Append(transform.DOScale(Vector3.zero, _duration * 0.7f).SetEase(Ease.InBack));
        sequence.Join(_text.DOFade(0f, _duration * 0.7f).SetEase(Ease.InQuad));
        sequence.Join(_material.DOFloat(0f, GlowPower, _duration * 0.7f).SetEase(Ease.InQuad));

        sequence.OnComplete(() => LeanPool.Despawn(gameObject));
    }

    public void ShowBonusCoin(int amount)
    {
        _text.text = "+" + amount.ToFormattedString();
        _text.alpha = 1f;
        _material.SetFloat(GlowPower, _initialGlowPower);
        transform.localScale = Vector3.zero;

        Vector3 targetPosition = transform.position + new Vector3(0f, _floatHeight * 1.5f, 0f);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOScale(_initialScale * 1.5f, _duration * 0.2f).SetEase(Ease.OutBack));
        sequence.Append(transform.DOMove(targetPosition, _duration).SetEase(Ease.OutCubic));
        sequence.Join(transform.DOScale(_initialScale, _duration * 0.3f).SetEase(Ease.InOutQuad));
        sequence.Join(_text.DOFade(0f, _duration).SetEase(Ease.InQuad));
        sequence.Join(_material.DOFloat(0f, GlowPower, _duration).SetEase(Ease.InQuad));

        sequence.OnComplete(() => LeanPool.Despawn(gameObject));
    }

    private void OnDisable()
    {
        DOTween.Kill(transform);
        DOTween.Kill(_text);
        DOTween.Kill(_material);
    }
}
