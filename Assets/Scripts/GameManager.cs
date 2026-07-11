using System;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        Menu,
        Playing,
        Death,
        GameOver,
    }
    
    public static event Action OnGameReset;
    public static event Action OnGameStart;
    public static event Action OnGameOver;

    private GameState currentGameState = GameState.Menu;

    void OnEnable()
    {
        SealInput.OnekeyPressed += SwitchGameState;
        Fish.OnPickupFish += OnPickupFish;
        Trash.OnPickupTrash += IncreaseHunger;
    }

    void OnDisable()
    {
        SealInput.OnekeyPressed -= SwitchGameState;
        Fish.OnPickupFish -= OnPickupFish;
        Trash.OnPickupTrash -= IncreaseHunger;
    }

    void Start()
    {
        ResetGame();
    }

    void Update()
    {
        if(currentGameState != GameState.Playing) return;

        HandleHunger();
    }

    private void SwitchGameState()
    {
        if(currentGameState == GameState.Menu) StartGame();
        else if(currentGameState == GameState.GameOver) ResetGame();
    }

    private void OnPickupFish(Fish fish)
    {
        IncreaseScore(fish.Points);
        IncreaseHunger(fish.Hunger);
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
    #region Hunger

    [SerializeField] private float maxHunger = 10;
    [SerializeField] private float hungerDecrease = 1;
    public float MaxHunger => maxHunger;
    public float Hunger
    {
        get => hunger;
        set => hunger = Mathf.Clamp(value, 0, maxHunger);
    }
    private float hunger;

    private void HandleHunger()
    {
        Hunger -= hungerDecrease * (CurrentVelocity / 2) * Time.deltaTime;
        if(Hunger == 0) StopGame();
    }

    private void IncreaseHunger(float hunger)
    {
        Hunger += hunger;
    }

    #endregion

    private void ResetGame()
    {
        StopAllCoroutines();
        currentGameState = GameState.Menu;

        CurrentVelocity = menuVelocity;
        Hunger = MaxHunger;

        OnGameReset?.Invoke();
    }

    private void StartGame()
    {
        currentGameState = GameState.Playing;
        
        Score = 0;

        StartCoroutine(IncreaseVelocity());

        OnGameStart?.Invoke();
    }

    private void StopGame()
    {
        currentGameState = GameState.Death;

        SaveSystem.SaveGame(new SaveSystem.SaveData(Score));
        OnGameOver?.Invoke();

        StartCoroutine(DeathTransition());
    }

    private IEnumerator DeathTransition()
    {
        yield return new WaitForSeconds(2);
        currentGameState = GameState.GameOver;
    }
}