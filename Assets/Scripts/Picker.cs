using UnityEngine;

public class Picker : MonoBehaviour
{
    #region Collisions

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.TryGetComponent(out IPickable pickable)) return;

        pickable.OnPickup();
    }

    #endregion
}