using UnityEngine;

public class AutoClicker : MonoBehaviour
{
    [SerializeField] private float _interval;

    private GameObject[] _clickables;
    private float _timer;

    private void Start()
    {
        _clickables = GameObject.FindGameObjectsWithTag("Planet");
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
            Damage = UpgradeManager.Instance.GetUpgrade(EUpgradeEffect.ClickPower)?.Value ?? 0
        };

        foreach (var clickable in _clickables)
        {
            clickable.GetComponent<IClickable>().OnClick(clickInfo);
        }
    }
}
