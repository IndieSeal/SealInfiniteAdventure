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

    [Header("Visuals")]
    [SerializeField] private Transform visualTransform;
    [SerializeField] private float visualRotateLerpSpeed = 15f;
    [SerializeField] private float maxAngle = 90;
    [SerializeField] private float zAngleOffset = -135;
    private float baseAngle;

    void Awake()
    {
        baseAngle = transform.eulerAngles.z;
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
    }

    void OnDisable()
    {
        SealInput.OnekeyPressed -= StartPressingKey;
        SealInput.OnekeyStopped -= StopPressingKey;
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
        rb.linearVelocityY = lerpedVelocity;
    }

    private void StopGliding()
    {
        if(!isBeingHeld && IsGrounded())
        {
            isGliding = false;
            rb.linearVelocityY = 0;
        }
    }

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
        if(groundedPoint == null) return;

        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundedPoint.position, groundedRadius);
    }

    #endregion
}