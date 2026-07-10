using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text fishCount;
    [SerializeField] private Image hungryBar;

    void OnEnable()
    {
        GameManager.OnScoreChanged += ScoreChanged;
    }

    private void ScoreChanged(int newScore)
    {
        fishCount.text = $"{newScore}";
    }
}