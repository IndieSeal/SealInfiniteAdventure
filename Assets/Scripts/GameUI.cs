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

    void OnEnable()
    {
        GameManager.OnGameReset += SetHighestScore;
        GameManager.OnScoreChanged += ScoreChanged;
    }

    void OnDisable()
    {
        GameManager.OnGameReset -= SetHighestScore;
        GameManager.OnScoreChanged -= ScoreChanged;
    }

    private void SetHighestScore()
    {
        highestScore = SaveSystem.GetGameData().highscore;
        highScoreFishCount.text = $"{highestScore}";
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