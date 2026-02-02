using DG.Tweening;
using UnityEngine;

public class LoginPlanetRotation : MonoBehaviour
{
    [SerializeField] private float secondsPerRevolution = 2f;

    private Tween _spinTween;
    private int _dir;

    private void OnEnable()
    {
        _dir = Random.value < 0.5f ? -1 : 1;
        StartSpin();
    }

    private void OnDisable()
    {
        _spinTween?.Kill();
        _spinTween = null;
    }

    private void StartSpin()
    {
        _spinTween?.Kill();

        _spinTween = transform
            .DOLocalRotate(
                new Vector3(0f, 0f, 360f * _dir),
                secondsPerRevolution,
                RotateMode.FastBeyond360
            )
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental)
            .SetUpdate(true);
    }
}
