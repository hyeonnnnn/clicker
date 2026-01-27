using UnityEngine;
using DG.Tweening;

public class ScaleTweeningFeedback : MonoBehaviour, IFeedback
{
    [SerializeField] private ClickTarget _owner;

    [Header("Punch Settings")]
    [SerializeField] private float _punchScale = 0.2f;
    [SerializeField] private float _duration = 0.3f;
    [SerializeField] private int _vibrato = 6;
    [SerializeField] private float _elasticity = 0.5f;

    private void Awake()
    {
        _owner = GetComponent<ClickTarget>();
    }

    public void Play(ClickInfo clickInfo)
    {
        if (clickInfo.Type == EClickType.Auto) return;

        _owner.transform.DOKill();
        _owner.transform.DOPunchScale(Vector3.one * _punchScale, _duration, _vibrato, _elasticity);
    }
}
