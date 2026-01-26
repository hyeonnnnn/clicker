using UnityEngine;

public class AutoClicker : MonoBehaviour
{
    [SerializeField] private float _interval;

    private GameObject[] _clickables;
    private float _timer;

    private void Start()
    {
        _clickables = GameObject.FindGameObjectsWithTag("Clickable");
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer < _interval) return;

        _timer = 0f;
        ClickAllTargets();
    }

    private void ClickAllTargets()
    {
        var clickInfo = new ClickInfo
        {
            Type = EClickType.Auto,
            Damage = GameManager.Instance.AutoDamage
        };

        foreach (var clickable in _clickables)
        {
            clickable.GetComponent<Clickable>().OnClick(clickInfo);
        }
    }
}
