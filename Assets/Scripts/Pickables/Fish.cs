using System;
using UnityEngine;
using uPools;

public class Fish : MonoBehaviour, IPickable
{
    public static event Action<int> OnPickupFish;
    
    [SerializeField] private int pointsOnPickup = 1;
    
    public void OnPickup()
    {
        OnPickupFish?.Invoke(pointsOnPickup);
        SharedGameObjectPool.Return(gameObject);
    }
}