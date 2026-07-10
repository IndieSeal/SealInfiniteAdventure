using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UVScroll
{
    public Renderer myRenderer;
    public Vector2 baseVelocity = new Vector2(1, 0);
    protected Vector2 currentOffset;

    public void Handle(float multiplier = 1)
    {
        currentOffset += baseVelocity * multiplier * Time.deltaTime;
        currentOffset = new Vector2(Mathf.Repeat(currentOffset.x, 1), Mathf.Repeat(currentOffset.y, 1));
        
        myRenderer.material.SetFloat("_OffsetUvX", currentOffset.x);
        myRenderer.material.SetFloat("_OffsetUvY", currentOffset.y);
    }
}

public class Sea : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> particleSystems;

    [Header("Sea Waves")]
    [SerializeField] private float waveSpeedMultiplier = 0.75f;
    [SerializeField] private List<UVScroll> waveUVScrolls = new List<UVScroll>();
    private float currentVelocityMultiplier = 1;

    void Awake()
    {
        GameManager.OnVelocityChanged += OnVelocityChanged;
    }

    void Update()
    {
        SeaWaves();
    }

    private void OnVelocityChanged(float velocityMultiplier)
    {
        currentVelocityMultiplier = velocityMultiplier;

        foreach(ParticleSystem ps in particleSystems)
        {
            var mainPs = ps.main;
            mainPs.simulationSpeed = currentVelocityMultiplier;
        }
    }

    private void SeaWaves()
    {
        waveUVScrolls.ForEach(x => x.Handle(currentVelocityMultiplier * waveSpeedMultiplier));
    }
}