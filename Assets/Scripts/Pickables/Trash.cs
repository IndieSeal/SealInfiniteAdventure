using System;
using UnityEngine;
using uPools;

public class Trash : MonoBehaviour, IPickable
{
    public static event Action<float> OnPickupTrash;
    
    [SerializeField] private float damageOnPickup = 2;
    [SerializeField] private bool returnToPoolOnPickup = true;

    [Header("Visuals")]
    [SerializeField] private ParticleSystem pickupParticles;

    public void OnPickup()
    {
        OnPickupTrash?.Invoke(damageOnPickup);

        SharedGameObjectPool.Rent(pickupParticles, transform.position, Quaternion.identity).Play();
        if(returnToPoolOnPickup) SharedGameObjectPool.Return(gameObject);
    }
}