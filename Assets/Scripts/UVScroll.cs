using UnityEngine;

public class UVScroll : MonoBehaviour
{
    [SerializeField] private Renderer myRenderer;
    [SerializeField] private Vector2 velocity = new Vector2(1, 0);
    private Vector2 currentOffset;

    void Update()
    {
        currentOffset += velocity * Time.deltaTime;
        myRenderer.material.SetFloat("_OffsetUvX", Mathf.Repeat(currentOffset.x, 1));
        myRenderer.material.SetFloat("_OffsetUvY", Mathf.Repeat(currentOffset.y, 1));
    }
}