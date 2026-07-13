using System.Collections;
using UnityEngine;
using uPools;

public class AlertManager : Singleton<AlertManager>
{
    [SerializeField] private Transform startPossible;
    [SerializeField] private Transform endPossible;
    [Space]
    [SerializeField] private GameObject alertPrefab;

    public void CreateAlert(Vector2 creaturePosition, float duration)
    {
        StartCoroutine(CreateAlertCoroutine(creaturePosition, duration));
    }

    private IEnumerator CreateAlertCoroutine(Vector2 creaturePosition, float duration)
    {
        yield return null;
        
        Vector2 spawnPosition = Utilities.GetRandomPoint(startPossible.position, endPossible.position);
        spawnPosition.y = creaturePosition.y;
        
        GameObject instance = SharedGameObjectPool.Rent(alertPrefab, spawnPosition, Quaternion.identity);
        yield return new WaitForSeconds(duration);
        SharedGameObjectPool.Return(instance);
    }
}