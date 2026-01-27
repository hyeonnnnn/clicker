using UnityEngine;
using TMPro;
using System.Collections;
using Lean.Pool;

public class DamageFloater : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;


    public void Show(ClickInfo clickInofo)
    {
        _text.text = clickInofo.Damage.ToString();

        StartCoroutine(Show_Coroutine());
    }

    private IEnumerator Show_Coroutine()
    {
        yield return new WaitForSeconds(0.7f);
        LeanPool.Despawn(gameObject);
    }
}
