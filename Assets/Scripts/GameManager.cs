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
    
    public static event Action OnLoseHunger;
    
    public static event Action OnGameReset;
    public static event Action OnGameStart;
    public static event Action OnGameOver;

    private GameState currentGameState = GameState.Menu;

    void OnEnable()
    {
        SealInput.OnekeyPressed += SwitchGameState;

        Fish.OnPickupFish += OnPickupFish;
        Trash.OnPickupTrash += OnPickupTrash;
    }

    void OnDisable()
    {
        SealInput.OnekeyPressed -= SwitchGameState;

        Fish.OnPickupFish -= OnPickupFish;
        Trash.OnPickupTrash -= OnPickupTrash;
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

    private void OnPickupTrash(float hunger)
    {        
        DecreaseHunger(hunger);
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
            Debug.Log(currentVelocity);
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

    [Header("Audio")]
    [SerializeField] private AudioSource pickupSound;
    [SerializeField] private AudioSource damageSound1;
    [SerializeField] private AudioSource damageSound2;
    [SerializeField] private AudioSource starvedSound;

    [Header("Hunger")]
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
        Hunger -= hungerDecrease * Time.deltaTime; //* (CurrentVelocity / 3)
        if(Hunger == 0) StopGame();
    }

    private void IncreaseHunger(float hunger)
    {
        Hunger += hunger;
        pickupSound.PlayVaried();
    }

    private void DecreaseHunger(float hunger)
    {
        int random = UnityEngine.Random.Range(0, 2);
        if(random == 0) damageSound1.PlayVaried();
        else if(random == 1) damageSound2.PlayVaried();
        else damageSound1.PlayVaried();
        
        Hunger -= hunger;

        OnLoseHunger?.Invoke();
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

        starvedSound.PlayVaried();

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