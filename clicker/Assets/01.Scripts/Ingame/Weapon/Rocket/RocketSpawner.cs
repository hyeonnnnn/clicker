using System.Collections.Generic;
using UnityEngine;

public class RocketSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _rocketPrefab;
    [SerializeField] private Transform _spawnParent;

    private readonly List<RocketController> _rockets = new List<RocketController>();
    private int _currentTurnIndex;

    private void Start()
    {
        SpawnController.Instance.RegisterRocketSpawner(this);
    }

    public void Spawn()
    {
        var rocket = Instantiate(_rocketPrefab, _spawnParent);
        var controller = rocket.GetComponent<RocketController>();

        controller.OnPassedCenter += AdvanceTurn;
        _rockets.Add(controller);
    }

    public void Redistribute()
    {
        int count = _rockets.Count;
        if (count <= 1) return;

        for (int i = 0; i < count; i++)
        {
            float offset = (2f * Mathf.PI * i) / count;
            _rockets[i].SetTimeOffset(offset);
        }
    }

    public float[] GetTimes()
    {
        var times = new float[_rockets.Count];
        for (int i = 0; i < _rockets.Count; i++)
            times[i] = _rockets[i].GetTime();
        return times;
    }

    public void SetTimes(float[] times)
    {
        if (times.Length != _rockets.Count)
        {
            Redistribute();
            return;
        }

        for (int i = 0; i < _rockets.Count; i++)
            _rockets[i].SetTimeOffset(times[i]);
    }

    private void AdvanceTurn()
    {
        if (_rockets.Count == 0) return;

        _currentTurnIndex = (_currentTurnIndex + 1) % _rockets.Count;
    }
}
