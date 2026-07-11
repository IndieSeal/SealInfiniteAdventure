using System;
using UnityEngine;

public class Trash : MonoBehaviour, IPickable
{
    public static event Action<float> OnPickupTrash;
    
    [SerializeField] private float damageOnPickup = -2;
    
    public void OnPickup()
    {
        OnPickupTrash?.Invoke(damageOnPickup);
        gameObject.SetActive(false);
    }
}