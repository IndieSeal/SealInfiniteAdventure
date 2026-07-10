using System;
using UnityEngine;

public class Trash : MonoBehaviour, IPickable
{
    public static event Action<int> OnPickupTrash;
    
    [SerializeField] private int damageOnPickup = 2;
    
    public void OnPickup()
    {
        OnPickupTrash?.Invoke(damageOnPickup);
        gameObject.SetActive(false);
    }
}