using System;
using System.Collections;
using UnityEngine;

public class Seal : MonoBehaviour
{
    public static event Action OnHitGround;
 
    private const string ANIMATION_STARVED_BOOL = "IsStarved";
    
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    private float originalY;
    private bool isStarved;

    [Header("Gliding")]
    [SerializeField] private float glidingVelocity = 1.4f;
    [SerializeField] private float glidingMaxUpwardsVelocity = 3;
    [SerializeField] private float glidingMaxDownwardsVelocity = -3;
    private bool isBeingHeld;
    private bool isGliding;

    private float glidingTargetVelocity;

    [Header("Boundaries")]
    [SerializeField] private Transform ceilledPoint;
    [SerializeField] private Transform groundedPoint;
    [SerializeField] private float groundedRadius = 0.1f;
    [SerializeField] private LayerMask groundedLayer;

    [Header("Visuals")]
    [SerializeField] private Transform visualTransform;
    [SerializeField] private float visualRotateLerpSpeed = 15f;
    [SerializeField] private float maxAngle = 90;
    [SerializeField] private float zAngleOffset = -135;

    void Awake()
    {
        originalY = transform.position.y;
    }

    void Update()
    {
        Glide();
        StopGliding();
        RotateVisual();
    }

    #region Input

    void OnEnable()
    {
        SealInput.OnekeyPressed += StartPressingKey;
        SealInput.OnekeyStopped += StopPressingKey;

        GameManager.OnGameReset += OnReset;
        GameManager.OnGameStart += OnStart;
        GameManager.OnGameOver += OnDeath;
    }

    void OnDisable()
    {
        SealInput.OnekeyPressed -= StartPressingKey;
        SealInput.OnekeyStopped -= StopPressingKey;

        GameManager.OnGameReset -= OnReset;
        GameManager.OnGameStart -= OnStart;
        GameManager.OnGameOver -= OnDeath;
    }

    private void OnReset()
    {
        isStarved = false;
        animator.SetBool(ANIMATION_STARVED_BOOL, isStarved);

        isGliding = false;
        StartCoroutine(GlideToDefault());
    }

    private void OnStart()
    {
        StopAllCoroutines();
    }

    private IEnumerator GlideToDefault()
    {
        while (true)
        {
            glidingTargetVelocity = originalY - transform.position.y;
            rb.linearVelocityY = glidingTargetVelocity * glidingVelocity;
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnDeath()
    {
        isStarved = true;
        animator.SetBool(ANIMATION_STARVED_BOOL, isStarved);

        isBeingHeld = false;
        isGliding = true;
    }

    private void StartPressingKey()
    {
        if(isStarved) return;

        StartGlide();
        isBeingHeld = true;
    }

    private void StopPressingKey()
    {
        if(isStarved) return;

        isBeingHeld = false;
    }

    #endregion
    #region Movement

    private void StartGlide()
    {        
        if(!isGliding) rb.linearVelocityY = 0;
        isGliding = true;
    }

    private void Glide()
    {
        if (!isGliding) return;

        glidingTargetVelocity = isBeingHeld ? glidingMaxUpwardsVelocity : glidingMaxDownwardsVelocity;

        float lerpedVelocity = Mathf.Lerp(rb.linearVelocityY, glidingTargetVelocity, glidingVelocity * Time.deltaTime);
        if(IsCeilled() && isBeingHeld) lerpedVelocity = 0;
        
        rb.linearVelocityY = lerpedVelocity;
    }

    private void StopGliding()
    {
        if(!isBeingHeld && isGliding && IsGrounded())
        {
            OnHitGround?.Invoke();
            isGliding = false;
            rb.linearVelocityY = 0;
        }
    }

    private bool IsCeilled() => Physics2D.OverlapCircle(ceilledPoint.position, groundedRadius, groundedLayer);
    private bool IsGrounded() => Physics2D.OverlapCircle(groundedPoint.position, groundedRadius, groundedLayer);

    #endregion
    #region Visual

    private void RotateVisual()
    {
        float value = Mathf.InverseLerp(glidingMaxDownwardsVelocity, glidingMaxUpwardsVelocity, rb.linearVelocityY);
        float zAngle = (maxAngle * value) + zAngleOffset;

        Quaternion targetAngle = Quaternion.Euler(visualTransform.eulerAngles.x, visualTransform.eulerAngles.y, zAngle);
        visualTransform.rotation = Quaternion.Lerp(visualTransform.rotation, targetAngle, visualRotateLerpSpeed * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        if(groundedPoint != null)
        {
            Gizmos.color = IsGrounded() ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundedPoint.position, groundedRadius);
        }
        if(ceilledPoint != null)
        {
            Gizmos.color = IsCeilled() ? Color.green : Color.red;
            Gizmos.DrawWireSphere(ceilledPoint.position, groundedRadius);
        }
    }

    #endregion
}