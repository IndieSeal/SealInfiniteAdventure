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

    [Header("Visuals")]
    [SerializeField] private ParticleSystem pickupParticles;
    
    public void OnPickup()
    {
        OnPickupFish?.Invoke(this);

        SharedGameObjectPool.Rent(pickupParticles, transform.position, Quaternion.identity).Play();
        SharedGameObjectPool.Return(gameObject);
    }
}