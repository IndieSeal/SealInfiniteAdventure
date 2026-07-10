using UnityEngine;

public class Seal : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Gliding")]
    [SerializeField] private float glidingVelocity = 1.4f;
    [SerializeField] private float glidingMaxUpwardsVelocity = 3;
    [SerializeField] private float glidingMaxDownwardsVelocity = -3;
    private bool isBeingHeld;
    private bool isGliding;

    private float glidingTargetVelocity;

    [Header("Grounded")]
    [SerializeField] private Transform groundedPoint;
    [SerializeField] private float groundedRadius = 0.1f;
    [SerializeField] private LayerMask groundedLayer;

    void Awake()
    {
        Reset();
    }

    void OnEnable()
    {
        SealInput.OnekeyPressed += StartPressingKey;
        SealInput.OnekeyStopped += StopPressingKey;
    }

    void OnDisable()
    {
        SealInput.OnekeyPressed -= StartPressingKey;
        SealInput.OnekeyStopped -= StopPressingKey;
    }

    private void Reset()
    {
        isGliding = false;
    }

    void Update()
    {
        Glide();
        StopGliding();
    }

    private void StartPressingKey()
    {
        StartGlide();
        isBeingHeld = true;
    }

    private void StopPressingKey()
    {
        isBeingHeld = false;
    }

    private void StartGlide()
    {
        if(!isGliding) rb.linearVelocityY = 0;
        isGliding = true;
    }

    private void Glide()
    {
        rb.bodyType = !isGliding ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
        if (!isGliding) return;

        glidingTargetVelocity = isBeingHeld ? glidingMaxUpwardsVelocity : glidingMaxDownwardsVelocity;

        float lerpedVelocity = Mathf.Lerp(rb.linearVelocityY, glidingTargetVelocity, glidingVelocity * Time.deltaTime);
        rb.linearVelocityY = lerpedVelocity;
    }

    private void StopGliding()
    {
        if(!isBeingHeld && IsGrounded()) isGliding = false;
    }

    private bool IsGrounded() => Physics2D.OverlapCircle(groundedPoint.position, groundedRadius, groundedLayer);

    void OnDrawGizmos()
    {
        if(groundedPoint == null) return;
        Gizmos.DrawWireSphere(groundedPoint.position, groundedRadius);
    }
}