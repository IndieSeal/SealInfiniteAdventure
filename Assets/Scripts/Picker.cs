using UnityEngine;

public class Picker : MonoBehaviour
{
    private bool canPickup;
    
    void OnEnable()
    {
        GameManager.OnGameStart += GameStarted;
        GameManager.OnGameOver += GameOver;
    }

    void OnDisable()
    {
        GameManager.OnGameStart -= GameStarted;
        GameManager.OnGameOver -= GameOver;
    }

    private void GameStarted()
    {
        canPickup = true;
    }

    private void GameOver()
    {
        canPickup = false;
    }

    #region Collisions

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!canPickup || !collision.TryGetComponent(out IPickable pickable)) return;

        pickable.OnPickup();
    }

    #endregion
}