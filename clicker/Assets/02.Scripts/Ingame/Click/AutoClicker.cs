using UnityEngine;

public class AutoClicker : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _interval;
    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _interval) 
        {
            _timer = 0f;

            GameObject[] clickables = GameObject.FindGameObjectsWithTag("Clickable");
            foreach (GameObject clickable in clickables)
            {
                Clickable clickableScript = clickable.GetComponent<Clickable>();
                ClickInfo clickInfo = new ClickInfo
                {
                    Type = EClickType.Auto,
                    Damage = _damage,
                };

                clickableScript.OnClick(clickInfo);
            }

        }
    }

}
