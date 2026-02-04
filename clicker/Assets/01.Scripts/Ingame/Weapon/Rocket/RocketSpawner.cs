using System.Collections.Generic;
using UnityEngine;

public class RocketSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _rocketPrefab;
    [SerializeField] private Transform _spawnParent;

    private readonly List<RocketMove> _rockets = new List<RocketMove>();
    private int _currentTurnIndex;

    public void Spawn()
    {
        var rocket = Instantiate(_rocketPrefab, _spawnParent);
        var rocketMove = rocket.GetComponent<RocketMove>();

        rocketMove.OnPassedCenter += () => AdvanceTurn();
        _rockets.Add(rocketMove);

    }

    private void AdvanceTurn()
    {
        if (_rockets.Count == 0) return;

        _currentTurnIndex = (_currentTurnIndex + 1) % _rockets.Count;
    }
}
