using System;
using UnityEngine;
using UnityEngine.InputSystem;

using static UnityEngine.InputSystem.InputAction;

public class SealInput : MonoBehaviour
{
    public const string PRESS_ACTION_NAME = "Press";
    
    public static event Action OnekeyPressed;
    public static event Action OnekeyHeld;
    public static event Action OnekeyStopped;

    [SerializeField] private PlayerInput playerInput;
    private InputAction pressAction;

    void Awake()
    {
        pressAction = playerInput.actions.FindAction(PRESS_ACTION_NAME);
    }

    void OnEnable()
    {
        pressAction.started += OnPressActionStarted;
        pressAction.performed += OnHoldActionStarted;
        pressAction.canceled += OnStoppedActionStarted;
    }

    void OnDisable()
    {
        pressAction.started -= OnPressActionStarted;
        pressAction.performed -= OnHoldActionStarted;
        pressAction.canceled -= OnStoppedActionStarted;
    }

    private void OnPressActionStarted(CallbackContext cbct) => OnekeyPressed?.Invoke();
    private void OnHoldActionStarted(CallbackContext cbct) => OnekeyHeld?.Invoke();
    private void OnStoppedActionStarted(CallbackContext cbct) => OnekeyStopped?.Invoke();
}