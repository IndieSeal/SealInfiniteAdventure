using System;
using UnityEngine;
using uPools;

public class Fish : MonoBehaviour, IPickable
{
    public static event Action<Fish> OnPickupFish;
    
    [SerializeField] private int pointsOnPickup = 1;
    public int Points => pointsOnPickup;

    [SerializeField] private float hungerRecovery = 1.3f;
    public float Hunger => hungerRecovery;
    
    public void OnPickup()
    {
        OnPickupFish?.Invoke(this);
        SharedGameObjectPool.Return(gameObject);
    }
}