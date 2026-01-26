using System.Collections;
using UnityEngine;

public class ColorFlashFeedback : MonoBehaviour, IFeedback
{
    private Coroutine _coroutine;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private Color _originalColor;
    [SerializeField] private Color _flashColor = Color.white;

    private void Start()
    {
        _originalColor = _spriteRenderer.color;
    }

    public void Play()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _coroutine = StartCoroutine(Play_Coroutine());
    }

    private IEnumerator Play_Coroutine()
    {
        _spriteRenderer.color = _flashColor;
        yield return new WaitForSeconds(0.3f);
        _spriteRenderer.color = _originalColor;
    }
}
