using UnityEngine;
using uPools;

[RequireComponent(typeof(ParticleSystem))]
public class PoolableParticles : MonoBehaviour
{
    private ParticleSystem particles;

    void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if(particles.IsAlive()) return;

        particles.Stop();
        SharedGameObjectPool.Return(gameObject);
    }
}