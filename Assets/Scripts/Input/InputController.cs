using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    PlayerInputAction inputAction;

    public Action OnPauseInput;

    private void Awake()
    {
        inputAction = new PlayerInputAction();
    }

    private void OnEnable()
    {
        inputAction.Enable();
        inputAction.UI.Pause.performed += OnPause;
    }

    private void OnDisable()
    {
        inputAction.UI.Pause.performed -= OnPause;
        inputAction.Disable();
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        OnPauseInput?.Invoke();
    }
}