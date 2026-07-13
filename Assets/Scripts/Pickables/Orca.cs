using System.Collections;
using UnityEngine;
using uPools;

public class Orca : Trash, IPoolCallbackReceiver
{
    [Header("Orca Movement")]
    [SerializeField, Tooltip("This value is normalized so don't expect speed to change if increased")] private Vector2 moveDirection = Vector2.left;
    [SerializeField] private float baseMinMovingSpeed = 5;
    [SerializeField] private float baseMaxMovingSpeed = 6;
    [SerializeField] private float waitDuration = 1;

    [Header("Sounds")]
    [SerializeField] private AudioSource growlSound;
    [SerializeField] private AudioSource attackSound;

    public void OnRent()
    {
        StartCoroutine(StartAttackCoroutine());
    }

    public void OnReturn()
    {
        StopAllCoroutines();
    }

    private IEnumerator StartAttackCoroutine()
    {
        float currentSeaVelocity = GameManager.Instance.CurrentVelocity;
        AlertManager.Instance.CreateAlert(transform.position, waitDuration);

        growlSound.PlayVaried();
        
        yield return new WaitForSeconds(waitDuration);

        growlSound.Stop();
        attackSound.PlayVaried();

        while (true)
        {
            Move(currentSeaVelocity);
            yield return null;            
        }    
    }

    private void Move(float currentSeaVelocity)
    {
        transform.position += (Vector3)moveDirection.normalized * (Random.Range(baseMinMovingSpeed, baseMaxMovingSpeed) * currentSeaVelocity * Time.deltaTime);
    }
}