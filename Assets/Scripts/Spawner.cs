using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uPools;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> fishPrefabs = new List<GameObject>();

    void Awake()
    {
        GameManager.OnGameStart += StartSpawning;
        GameManager.OnGameOver += EndSpawning;
    }

    private void StartSpawning()
    {
        StartCoroutine(SpawnFishCoroutine());
    }

    private void EndSpawning()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnFishCoroutine()
    {
        while (true)
        {
            SharedGameObjectPool.Rent(fishPrefabs.GetRandomOf(), transform.position, Quaternion.identity);
            yield return new WaitForSeconds(5);
        }
    }
}