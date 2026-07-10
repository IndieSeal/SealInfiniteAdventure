using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action OnGameStart;
    public static event Action<int> OnGameOver;

    void Start()
    {
        OnGameStart?.Invoke();
    }

    void Update()
    {
        
    }
}