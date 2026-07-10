using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action<float> OnVelocityChanged;
    
    public static event Action OnGameReset;
    public static event Action OnGameStart;
    public static event Action<int> OnGameOver;

    private float timer;

    [SerializeField] private float maxVelocity = 2.5f;
    [Space]
    [SerializeField] private float velocityPerDelay = 0.1f;
    [SerializeField] private float velocityDelay = 5;
    private float currentVelocity = 1;

    void Start()
    {
        ResetGame();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > velocityDelay)
        {
            timer = 0;
            
            currentVelocity = Mathf.Clamp(currentVelocity + velocityPerDelay, 0, maxVelocity);
            OnVelocityChanged?.Invoke(currentVelocity);
        }
    }

    private void ResetGame()
    {
        OnGameReset?.Invoke();

        currentVelocity = 1;
        OnVelocityChanged?.Invoke(currentVelocity);
    }
}