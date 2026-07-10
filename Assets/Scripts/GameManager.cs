using System;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{    
    public static event Action OnGameReset;
    public static event Action OnGameStart;
    public static event Action OnGameOver;

    protected override void Awake()
    {
        base.Awake();
        
        Fish.OnPickupFish += IncreaseScore;
    }

    void Start()
    {
        ResetGame();
        StartGame();
    }

    #region Game's Velocity

    public static event Action<float> OnVelocityChanged;

    [SerializeField] private float menuVelocity = 0.1f;
    [SerializeField] private float minVelocity = 1;
    [SerializeField] private float maxVelocity = 2.5f;
    [Space]
    [SerializeField] private float velocityPerDelay = 0.1f;
    [SerializeField] private float velocityDelay = 5;

    public float CurrentVelocity
    {
        get => currentVelocity;
        private set
        {
            currentVelocity = value;
            OnVelocityChanged?.Invoke(currentVelocity);
        }
    }
    private float currentVelocity;

    private IEnumerator IncreaseVelocity()
    {
        CurrentVelocity = minVelocity;

        while (true)
        {
            if(CurrentVelocity >= maxVelocity) break;
        
            yield return new WaitForSeconds(velocityDelay);

            CurrentVelocity = Mathf.Clamp(CurrentVelocity + velocityPerDelay, minVelocity, maxVelocity);
        }
    }

    #endregion
    #region Points
    
    public static event Action<int> OnScoreChanged;
    public int Score
    {
        get => score;
        set
        {
            score = value;
            OnScoreChanged?.Invoke(score);
        }
    }
    private int score;

    private void IncreaseScore(int score)
    {
        Score += score;
    }

    #endregion

    private void ResetGame()
    {
        CurrentVelocity = menuVelocity;
        OnGameReset?.Invoke();
    }

    private void StartGame()
    {
        Score = 0;

        StartCoroutine(IncreaseVelocity());

        OnGameStart?.Invoke();
    }

    private void StopGame()
    {
        SaveSystem.SaveGame(new SaveSystem.SaveData(Score));

        OnGameOver?.Invoke();
    }
}