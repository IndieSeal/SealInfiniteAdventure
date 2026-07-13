using System;
using UnityEngine;
using uPools;

public class Trash : MonoBehaviour, IPickable
{
    public static event Action<float> OnPickupTrash;
    
    [SerializeField] private float damageOnPickup = 2;
    [SerializeField] private bool returnToPoolOnPickup = true;

    public void OnPickup()
    {
        OnPickupTrash?.Invoke(damageOnPickup);
        if(returnToPoolOnPickup) SharedGameObjectPool.Return(gameObject);
    }
}