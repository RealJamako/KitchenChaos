using System;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance { get; private set; }

    public event Action OnInteract;
    public event Action OnInteractAlt;
    public event Action OnPause;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new();
    }

    private void Start()
    {
        InstanceCheck();
    }

    private void OnEnable()
    {
        inputActions.Enable();

        //Subscribe to events
        inputActions.Gameplay.Interact.performed += Interact_performed;
        inputActions.Gameplay.InteractAlt.performed += InteractAlt_performed;
        inputActions.Gameplay.Pause.performed += Pause_performed;
    }

    private void OnDisable()
    {
        inputActions.Disable();

        //Unsubscribe to events
        inputActions.Gameplay.Interact.performed -= Interact_performed;
        inputActions.Gameplay.InteractAlt.performed -= InteractAlt_performed;
        inputActions.Gameplay.Pause.performed -= Pause_performed;
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteract?.Invoke();
    }

    private void InteractAlt_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlt?.Invoke();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPause?.Invoke();
    }

    public Vector3 GetMovementNormalized()
    {
        Vector2 playerInputDirection2D = inputActions.Gameplay.Move.ReadValue<Vector2>();
        Vector3 inputDirection = new(playerInputDirection2D.x, 0, playerInputDirection2D.y);
        return inputDirection.normalized;
    }

    public string GetBindingText(KeyBindings keyBinding)
    {
        return keyBinding switch
        {
            KeyBindings.ActionA => inputActions.Gameplay.Interact.bindings[0].ToDisplayString(),
            KeyBindings.ActionB => inputActions.Gameplay.InteractAlt.bindings[0].ToDisplayString(),
            KeyBindings.Pause => inputActions.Gameplay.Pause.bindings[0].ToDisplayString(),
            _ => string.Empty,
        };
    }

    public void RebindBinding(KeyBindings keyBinding, Action<bool> onActionRebound)
    {
        inputActions.Gameplay.Disable();

        InputAction inputAction = keyBinding switch
        {
            KeyBindings.ActionB => inputActions.Gameplay.InteractAlt,
            KeyBindings.Pause => inputActions.Gameplay.Pause,
            _ => inputActions.Gameplay.Interact,
        };

        inputAction.PerformInteractiveRebinding(0)
            .OnComplete(callback =>
            {
                callback.Dispose();
                inputActions.Gameplay.Enable();
                onActionRebound(false);
            })
            .Start();
    }

    private void InstanceCheck()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one player instance");
            return;
        }
        else
        {
            Instance = this;
        }
    }
}