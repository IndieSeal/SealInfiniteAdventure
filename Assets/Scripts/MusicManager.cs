using System.Collections;
using UnityEngine;

[System.Serializable]
public class MusicFilter
{
    public float highFilter = 1500;
    public float lowFilter = 4000;
}

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioLowPassFilter lowFilter;
    [SerializeField] private AudioHighPassFilter highFilter;
    [SerializeField] private float filterChangeSpeed = 1000;
    
    [Header("Filters")]
    [SerializeField] private MusicFilter menuFilter;
    [SerializeField] private MusicFilter gameFilter;
    [SerializeField] private MusicFilter loseFilter;

    void OnEnable()
    {
        GameManager.OnGameReset += ApplyMenuFilter;
        GameManager.OnGameStart += ApplyGameFilter;
        GameManager.OnGameOver += ApplyLoseFilter;
    }

    void OnDisable()
    {
        GameManager.OnGameReset -= ApplyMenuFilter;
        GameManager.OnGameStart -= ApplyGameFilter;
        GameManager.OnGameOver -= ApplyLoseFilter;
    }

    private void ApplyMenuFilter() => ApplyFilter(menuFilter, filterChangeSpeed);
    private void ApplyGameFilter() => ApplyFilter(gameFilter, filterChangeSpeed);
    private void ApplyLoseFilter() => ApplyFilter(loseFilter, filterChangeSpeed);

    private void ApplyFilter(MusicFilter filter, float velocity)
    {
        StopAllCoroutines();
        StartCoroutine(ApplyFilterCoroutine(filter, velocity));
    }

    private IEnumerator ApplyFilterCoroutine(MusicFilter filter, float velocity)
    {
        while (true)
        {
            lowFilter.cutoffFrequency = Mathf.Lerp(lowFilter.cutoffFrequency, filter.lowFilter, velocity * Time.deltaTime);
            highFilter.cutoffFrequency = Mathf.Lerp(highFilter.cutoffFrequency, filter.highFilter, velocity * Time.deltaTime);
            
            if(Mathf.Abs(filter.lowFilter - lowFilter.cutoffFrequency) < 10 && Mathf.Abs(filter.highFilter - highFilter.cutoffFrequency) < 10) break;
            yield return null;
        }
    }
}