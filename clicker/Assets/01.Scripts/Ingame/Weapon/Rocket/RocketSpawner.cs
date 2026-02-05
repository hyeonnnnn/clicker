using System.Collections.Generic;
using UnityEngine;

public class RocketSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _rocketPrefab;
    [SerializeField] private Transform _spawnParent;

    private readonly List<RocketController> _rockets = new List<RocketController>();
    private int _currentTurnIndex;

    public void Spawn()
    {
        var rocket = Instantiate(_rocketPrefab, _spawnParent);
        var controller = rocket.GetComponent<RocketController>();

        controller.OnPassedCenter += AdvanceTurn;
        _rockets.Add(controller);
    }

    private void AdvanceTurn()
    {
        if (_rockets.Count == 0) return;

        _currentTurnIndex = (_currentTurnIndex + 1) % _rockets.Count;
    }
}
