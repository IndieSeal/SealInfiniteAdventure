using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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
    [SerializeField] private float hungerFilterThreshold = 0.4f;
    
    //this really doesn't belong here but we manage certain things here which are needed for that, soooo...
    [Header("Visuals")]
    [SerializeField] private Volume postProcessing;
    private VolumeProfile postProcessingProfile;
    private Vignette vignette;
    private ChromaticAberration chrAberration;
    
    [Header("Filters")]
    [SerializeField] private MusicFilter menuFilter;
    [SerializeField] private MusicFilter loseFilter;
    [Space]
    [SerializeField] private MusicFilter gameFilter;
    [SerializeField] private MusicFilter hungerFilter;
    
    private Coroutine currentCoroutine;

    void Awake()
    {
        postProcessingProfile = Instantiate(postProcessing.profile);
        postProcessing.profile = postProcessingProfile;
        
        postProcessingProfile.TryGet(out vignette);
        postProcessingProfile.TryGet(out chrAberration);
    }

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

    void Update()
    {
        if(currentCoroutine == null)
        {
            float value = GameManager.Instance.Hunger / (GameManager.Instance.MaxHunger * hungerFilterThreshold);

            lowFilter.cutoffFrequency = Mathf.Lerp(hungerFilter.lowFilter, gameFilter.lowFilter, value);
            highFilter.cutoffFrequency = Mathf.Lerp(hungerFilter.highFilter, gameFilter.highFilter, value);

            vignette.intensity.value = Mathf.Lerp(0.35f, 0.15f, value);
            chrAberration.intensity.value = Mathf.Lerp(0.25f, 0f, value);
        }
    }

    private void SetFullPostProcessValues()
    {
        vignette.intensity.value = 0.35f;
        chrAberration.intensity.value = 0.25f;
    }

    private void ResetPostProcessValues()
    {
        vignette.intensity.value = 0.15f;
        chrAberration.intensity.value = 0f;
    }

    private void ApplyMenuFilter()
    {
        ResetPostProcessValues();
        ApplyFilter(menuFilter, filterChangeSpeed, false);
    }
    private void ApplyGameFilter() => ApplyFilter(gameFilter, filterChangeSpeed, true);
    private void ApplyLoseFilter()
    {
        SetFullPostProcessValues();
        ApplyFilter(loseFilter, filterChangeSpeed, false);
    }

    private void ApplyFilter(MusicFilter filter, float velocity, bool overridable = true)
    {
        if(currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(ApplyFilterCoroutine(filter, velocity, overridable));
    }

    private IEnumerator ApplyFilterCoroutine(MusicFilter filter, float velocity, bool overridable = true)
    {
        while (true)
        {
            lowFilter.cutoffFrequency = Mathf.Lerp(lowFilter.cutoffFrequency, filter.lowFilter, velocity * Time.deltaTime);
            highFilter.cutoffFrequency = Mathf.Lerp(highFilter.cutoffFrequency, filter.highFilter, velocity * Time.deltaTime);
            
            if(Mathf.Abs(filter.lowFilter - lowFilter.cutoffFrequency) < 10 && Mathf.Abs(filter.highFilter - highFilter.cutoffFrequency) < 10 && overridable)
            {
                currentCoroutine = null;
                break;
            }
            yield return null;
        }
    }
}