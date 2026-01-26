using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [SerializeField] private List<RockAttack> _rocks;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SpawnRock();
        }
    }

    private void SpawnRock()
    {
        // 화면 내 랜덤한 위치, 랜덤한 방향으로 rock 스폰
    }
}
