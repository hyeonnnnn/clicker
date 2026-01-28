using System.Collections.Generic;
using UnityEngine;

public class ShurikenAttackerSelector : MonoBehaviour
{
    [SerializeField] private List<RocketAttack> _shurikens;
    [SerializeField] private float _interval = 3f;

    private float _timer;
    private int _currentIndex;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _interval)
        {
            _timer = 0f;
            SelectAndAttack();
        }
    }

    private void SelectAndAttack()
    {
        var activeList = GetActiveShurikens();
        if (activeList.Count == 0) return;

        _currentIndex %= activeList.Count;
        activeList[_currentIndex].Attack();
        _currentIndex = (_currentIndex + 1) % activeList.Count;
    }

    private List<RocketAttack> GetActiveShurikens()
    {
        var activeList = new List<RocketAttack>();
        foreach (var rocket in _shurikens)
        {
            if (rocket.gameObject.activeInHierarchy)
            {
                activeList.Add(rocket);
            }
        }
        return activeList;
    }
}
