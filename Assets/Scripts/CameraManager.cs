using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform cameraShaker;
    [SerializeField] private Transform deathPosition;
    [SerializeField] private float deathFOV = 5;
 
    private Coroutine lastShakeCoroutine;
    private Coroutine lastCameraCoroutine;
    
    private Camera myCamera;
    private Vector2 defaultPosition;
    private float defaultFOV;
    
    void Awake()
    {
        myCamera = GetComponent<Camera>();

        defaultPosition = transform.position;
        defaultFOV = myCamera.orthographicSize;
    }

    void OnEnable()
    {
        GameManager.OnGameReset += ResetToDefault;
        GameManager.OnGameOver += TransitionToDeath;

        Trash.OnPickupTrash += TrashCameraShake;
    }

    void OnDisable()
    {
        GameManager.OnGameReset -= ResetToDefault;
        GameManager.OnGameOver -= TransitionToDeath;

        Trash.OnPickupTrash -= TrashCameraShake;
    }

    public void TrashCameraShake(float hunger) => ShakeCamera(0.15f, 0.6f);
    public void BigCameraShake() => ShakeCamera(0.45f, 1f);

    public void ShakeCamera(float shakeDuration, float shakeForce)
    {
        if(lastShakeCoroutine != null) StopCoroutine(lastShakeCoroutine);
        lastShakeCoroutine = StartCoroutine(ShakeCameraCoroutine(shakeDuration, shakeForce));
    }

    private void ResetToDefault()
    {
        if(lastCameraCoroutine != null) StopCoroutine(lastCameraCoroutine);
        lastCameraCoroutine = StartCoroutine(MoveCameraCoroutine(defaultPosition, defaultFOV));
    }

    private void TransitionToDeath()
    {
        BigCameraShake();
        
        if(lastCameraCoroutine != null) StopCoroutine(lastCameraCoroutine);
        lastCameraCoroutine = StartCoroutine(MoveCameraCoroutine(deathPosition.position, deathFOV));
    }

    private IEnumerator MoveCameraCoroutine(Vector2 targetPosition, float targetFOV)
    {
        float originalZ = cameraShaker.position.z;
        
        while (true)
        {
            Vector3 targetPos = Vector2.Lerp(cameraShaker.position, targetPosition, 1 * Time.deltaTime);
            targetPos.z = originalZ;
            cameraShaker.position = targetPos;
            
            myCamera.orthographicSize = Mathf.Lerp(myCamera.orthographicSize, targetFOV, 1 * Time.deltaTime);

            if(Mathf.Abs(targetFOV - myCamera.orthographicSize) < 0.02f && Vector2.Distance(cameraShaker.position, targetPosition) < 0.02f) break;
            yield return null;
        }
    }

    private IEnumerator ShakeCameraCoroutine(float shakeDuration, float shakeForce)
    {
        Vector3 originalPosition = myCamera.transform.localPosition;
        while (shakeDuration > 0)
        {
            Vector3 randomPosition = Random.insideUnitCircle * shakeForce;
            randomPosition.z = originalPosition.z;
            
            myCamera.transform.localPosition = randomPosition;

            shakeDuration -= Time.deltaTime;
            yield return null;
        }

        myCamera.transform.localPosition = originalPosition;
    }
}