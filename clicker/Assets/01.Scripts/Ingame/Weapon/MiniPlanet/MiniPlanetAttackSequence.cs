using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniPlanetAttackSequence : MonoBehaviour
{
    [SerializeField] private float _cooldown = 5f;
    [SerializeField] private float _attackInterval = 0.2f;

    private List<MiniPlanetAttack> _miniPlanets = new List<MiniPlanetAttack>();
    private float _timer;
    private bool _isAttacking;

    private void Update()
    {
        if (_miniPlanets.Count == 0) return;
        if (_isAttacking) return;

        _timer += Time.deltaTime;

        if (_timer >= _cooldown)
        {
            _timer = 0f;
            StartCoroutine(AttackSequence());
        }
    }

    private IEnumerator AttackSequence()
    {
        _isAttacking = true;

        var activeList = GetActiveMiniPlanets();

        foreach (var miniPlanet in activeList)
        {
            miniPlanet.Attack();
            yield return new WaitForSeconds(_attackInterval);
        }

        _isAttacking = false;
    }

    private List<MiniPlanetAttack> GetActiveMiniPlanets()
    {
        var activeList = new List<MiniPlanetAttack>();
        foreach (var miniPlanet in _miniPlanets)
        {
            if (miniPlanet.gameObject.activeInHierarchy)
            {
                activeList.Add(miniPlanet);
            }
        }
        return activeList;
    }

    public void AddMiniPlanet(MiniPlanetAttack miniPlanet)
    {
        _miniPlanets.Add(miniPlanet);
    }
}
