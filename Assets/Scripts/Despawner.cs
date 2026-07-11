using System;
using UnityEngine;

public class Despawner : MonoBehaviour
{
    public static event Action<GameObject> OnDespawnObject;

    void OnTriggerEnter2D(Collider2D collision)
    {
        OnDespawnObject?.Invoke(collision.gameObject);
    }
}