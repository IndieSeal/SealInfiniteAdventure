using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreFishCount;
    [SerializeField] private TMP_Text fishCount;
    [SerializeField] private Image hungryBar;
    [SerializeField] private float hungryLerpSpeed = 1;
    private int highestScore;

    [Space]
    [SerializeField] private TMP_Text pressTo;

    void OnEnable()
    {
        GameManager.OnGameReset += SetHighestScore;
        GameManager.OnGameOver += OnGameOver;
        GameManager.OnGameStart += StartGame;
        
        GameManager.OnScoreChanged += ScoreChanged;
    }

    void OnDisable()
    {
        GameManager.OnGameReset -= SetHighestScore;
        GameManager.OnGameOver -= OnGameOver;
        GameManager.OnGameStart -= StartGame;
        
        GameManager.OnScoreChanged -= ScoreChanged;
    }

    private void OnGameOver()
    {
        pressTo.text = "Press 'Space' to start game";
    }

    private void SetHighestScore()
    {        
        highestScore = SaveSystem.GetGameData().highscore;
        highScoreFishCount.text = $"{highestScore}";
    }

    private void StartGame()
    {
        pressTo.text = "";
    }

    void Update()
    {
        HungerChanged();
    }

    private void ScoreChanged(int newScore)
    {
        if(newScore > highestScore) highScoreFishCount.text = $"{newScore}";
        fishCount.text = $"{newScore}";
    }

    private void HungerChanged()
    {
        float currentHunger = GameManager.Instance.Hunger;
        hungryBar.fillAmount = Mathf.Lerp(hungryBar.fillAmount, currentHunger/GameManager.Instance.MaxHunger, hungryLerpSpeed * Time.deltaTime);
    }
}