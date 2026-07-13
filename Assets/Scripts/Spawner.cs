using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uPools;

[System.Serializable]
public class SpawnContent
{
    public List<GameObject> prefabs = new List<GameObject>();

    public Vector2 offset;

    [Header("Spawn Delay")]
    [Tooltip("If true, the sea's velocity will affect the spawning time of this")] public bool seaAffected = true;
    [Space]
    public float minSpawnTime = 1;
    public float maxSpawnTime = 2;

    [Tooltip("Which sea velocity you have to be in for it to start spawning")] public float spawnAfterVelocity = 0;

    public GameObject Prefab => prefabs.GetRandomOf();
    public float Delay => Random.Range(minSpawnTime, maxSpawnTime) / (!seaAffected ? 1 : GameManager.Instance.CurrentVelocity);
}

public class Spawner : MonoBehaviour
{
    [Header("Spawning")]
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform endTransform;
    
    [SerializeField] private List<SpawnContent> spawns = new List<SpawnContent>();
    private List<GameObject> instances = new List<GameObject>();

    void OnEnable()
    {
        GameManager.OnGameStart += StartSpawning;
        GameManager.OnGameOver += EndSpawning;

        Despawner.OnDespawnObject += DespawnObject;
    }

    void OnDisable()
    {
        GameManager.OnGameStart -= StartSpawning;
        GameManager.OnGameOver -= EndSpawning;

        Despawner.OnDespawnObject -= DespawnObject;
    }

    private void StartSpawning()
    {
        foreach(SpawnContent spawn in spawns) StartCoroutine(SpawnerCoroutine(spawn));
    }

    private void EndSpawning()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnerCoroutine(SpawnContent spawn)
    {
        while (true)
        {
            yield return new WaitForSeconds(spawn.Delay);

            if(GameManager.Instance.CurrentVelocity < spawn.spawnAfterVelocity) continue;
            instances.Add(SharedGameObjectPool.Rent(spawn.Prefab, Utilities.GetRandomPoint(startTransform.position, endTransform.position) + spawn.offset, Quaternion.identity));
        }
    }

    private void DespawnObject(GameObject go)
    {
        if(instances.Contains(go)) SharedGameObjectPool.Return(go);
    }
}