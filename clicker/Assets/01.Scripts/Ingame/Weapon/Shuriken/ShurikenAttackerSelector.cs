using System.Collections.Generic;
using UnityEngine;

public class ShurikenAttackerSelector : MonoBehaviour
{
    [SerializeField] private List<ShurikenAttack> _shurikens;
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

    private List<ShurikenAttack> GetActiveShurikens()
    {
        var activeList = new List<ShurikenAttack>();
        foreach (var shuriken in _shurikens)
        {
            if (shuriken.gameObject.activeInHierarchy)
            {
                activeList.Add(shuriken);
            }
        }
        return activeList;
    }
}
