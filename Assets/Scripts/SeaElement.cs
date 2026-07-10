using UnityEngine;
using uPools;

public class SeaElement : MonoBehaviour, IPoolCallbackReceiver
{
    [SerializeField] private Vector2 moveDirection = Vector2.left;
    private float currentVelocity;

    void Update()
    {
        transform.position += (Vector3)moveDirection * (currentVelocity * Time.deltaTime);
    }

    public void OnRent() => currentVelocity = GameManager.Instance.CurrentVelocity;
    public void OnReturn() => currentVelocity = 0;
}