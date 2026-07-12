using UnityEngine;
using uPools;

public class Squid : MonoBehaviour, IPoolCallbackReceiver
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private AudioSource inkSound;

    [Header("Particles")]
    [SerializeField] private ParticleSystem inkParticles;
    private Transform originalParent;
    private Vector2 originalPosition;
    
    [Header("Jumping")]
    [SerializeField] private float minJumpInterval = 0.6f;
    [SerializeField] private float maxJumpInterval = 5.6f;
    [Space]
    [SerializeField] private float jumpForce = 5;
    private float jumpTimer;

    [Header("Velocity")]
    [SerializeField] private float maximumDownwardsVelocity = -0.5f;

    void Awake()
    {
        originalParent = inkParticles.transform.parent;
        originalPosition = inkParticles.transform.localPosition;
    }

    void Update()
    {
        HandleJumping();
        HandleVelocity();
    }

    private void HandleJumping()
    {
        jumpTimer -= Time.deltaTime;
        if(jumpTimer <= 0) OnJump();
    }

    private void OnJump()
    {
        RandomizeJumpTimer();
        
        rb.AddForceY(jumpForce, ForceMode2D.Impulse);
        inkSound.Play();

        inkParticles.transform.SetParent(originalParent);
        inkParticles.transform.localPosition = originalPosition;

        inkParticles.transform.SetParent(null);
        inkParticles.Play();
    }

    private void HandleVelocity()
    {
        if(rb.linearVelocityY < maximumDownwardsVelocity) rb.linearVelocityY = maximumDownwardsVelocity;
    }

    private void RandomizeJumpTimer() => jumpTimer = Random.Range(minJumpInterval, maxJumpInterval);

    public void OnRent()
    {
        RandomizeJumpTimer();
    }

    public void OnReturn(){}
}