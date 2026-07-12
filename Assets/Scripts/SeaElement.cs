using UnityEngine;
using uPools;

public class SeaElement : MonoBehaviour, IPoolCallbackReceiver
{
    [SerializeField, Tooltip("This value is normalized so don't expect speed to change if increased")] private Vector2 moveDirection = Vector2.left;
    [SerializeField] private float minVelocity = 4;
    [SerializeField] private float maxVelocity = 5;
    private float seaVelocity;

    void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.position += (Vector3)moveDirection.normalized * (Random.Range(minVelocity, maxVelocity) * seaVelocity * Time.deltaTime);
    }

    public void OnRent() => seaVelocity = GameManager.Instance.CurrentVelocity;
    public void OnReturn() => seaVelocity = 0;
}