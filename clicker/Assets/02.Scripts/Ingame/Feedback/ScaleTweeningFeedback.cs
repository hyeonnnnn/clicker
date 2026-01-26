using UnityEngine;
using DG.Tweening;

public class ScaleTweeningFeedback : MonoBehaviour, IFeedback
{
    [SerializeField] private ClickTarget _owner;

    private void Awake()
    {
        _owner = GetComponent<ClickTarget>();
    }

    public void Play(ClickInfo clickInfo)
    {
        if (clickInfo.Type == EClickType.Auto) return;

        _owner.transform.DOKill();
        _owner.transform.DOScale(1.2f, 0.6f).OnComplete(() =>
        {
            _owner.transform.localScale = Vector3.one;
        });
    }
}
