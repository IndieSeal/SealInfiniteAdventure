using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform deathPosition;
    [SerializeField] private float deathFOV = 5;
    
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
    }

    void OnDisable()
    {
        GameManager.OnGameReset -= ResetToDefault;
        GameManager.OnGameOver -= TransitionToDeath;
    }

    private void ResetToDefault()
    {
        StopAllCoroutines();
        StartCoroutine(MoveCameraCoroutine(defaultPosition, defaultFOV));
    }

    private void TransitionToDeath()
    {
        StopAllCoroutines();
        StartCoroutine(MoveCameraCoroutine(deathPosition.position, deathFOV));
    }

    private IEnumerator MoveCameraCoroutine(Vector2 targetPosition, float targetFOV)
    {
        float originalZ = transform.position.z;
        
        while (true)
        {
            Vector3 targetPos = Vector2.Lerp(transform.position, targetPosition, 1 * Time.deltaTime);
            targetPos.z = originalZ;
            transform.position = targetPos;
            
            myCamera.orthographicSize = Mathf.Lerp(myCamera.orthographicSize, targetFOV, 1 * Time.deltaTime);

            if(Mathf.Abs(targetFOV - myCamera.orthographicSize) < 0.02f && Vector2.Distance(transform.position, targetPosition) < 0.02f) break;
            yield return null;
        }
    }
}