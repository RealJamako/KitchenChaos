using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour, IKitchenObjectParent
{
    public static PlayerController Instance { get; private set; }
    public event EventHandler OnPlayerPickUp;

    public KitchenObject KitchenObject { 
        get { return kitchenObject; }
        set 
        {
            if (value != null)
            {
                OnPlayerPickUp?.Invoke(this, EventArgs.Empty);
            }
            kitchenObject = value;
        } 
    }


    public event EventHandler<OnSelectedCounterChangedArgs> OnSelectedCounterChange;

    [Header("Movement Settings")]
    [Tooltip("How fast does the player move?")]
    [SerializeField] private float moveSpeed;

    [Space(5f)]
    [Header("Visual Settings")]
    [Tooltip("How much smoothing is applied to the rotation of the player")]
    [SerializeField] private float rotationSmoothing;

    [Space(5f)]
    [Header("Interaction Settings")]
    [Tooltip("How long should the raycast be?")]
    [SerializeField] private float rayLength;
    [Tooltip("How far off the ground should the raycast be?")]
    [SerializeField] private float rayGroundOffset;
    [Tooltip("Which layer should the raycast look for?")]
    [SerializeField] private LayerMask rayLayerMask;

    [Space(5f)]
    [Header("Player Hold Position")]
    [Tooltip("Where the object will parent to when holding an item")]
    [SerializeField] private Transform holdPoint;

    private PlayerInput input;
    private CharacterController characterController;

    private BaseCounter selectedCounter;

    private KitchenObject kitchenObject;

    private void Awake()
    {
        InstanceCheck();
        input = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        input.OnInteract += Input_OnInteract;
        input.OnInteractAlt += Input_OnInteractAlt;
    }

    private void OnDisable()
    {
        input.OnInteract -= Input_OnInteract;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleInteractions();
    }

    private void HandleMovement()
    {
        characterController.Move(moveSpeed * Time.deltaTime * input.GetMovementNormalized());
    }

    private void HandleRotation()
    {
        Vector3 target = Vector3.Slerp(transform.forward, input.GetMovementNormalized(), rotationSmoothing * Time.deltaTime);
        if (target == Vector3.zero) { return; }
        transform.forward = target;
    }

    private void HandleInteractions()
    {
        bool hitObject = Physics.Raycast(InteractionRaycastPosition(), transform.forward, out RaycastHit hitObjectInfo, rayLength, rayLayerMask);
        if (hitObject)
        {
            if (hitObjectInfo.transform.TryGetComponent(out BaseCounter clearCounter))
            {
                if (clearCounter != selectedCounter)
                {
                    SetSelectedCounter(clearCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void Input_OnInteract()
    {
        if (!GameManager.Instance.IsGamePlaying()) { return; }
        if (selectedCounter == null) { return; }
        selectedCounter.Interact(this);
    }

    private void Input_OnInteractAlt()
    {
        if (!GameManager.Instance.IsGamePlaying()) { return; }
        if (selectedCounter == null) { return; }
        selectedCounter.InteractAlt(this);
    }

    public bool IsWalking()
    {
        return input.GetMovementNormalized().magnitude > Mathf.Epsilon;
    }

    private Vector3 InteractionRaycastPosition()
    {
        return new Vector3(transform.position.x, transform.position.y + rayGroundOffset, transform.position.z);
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChange?.Invoke(this, new OnSelectedCounterChangedArgs
        {
            SelectedCounter = selectedCounter,
        });
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

    private void OnDrawGizmosSelected()
    {
        //Draw a debug ray to show interaction raycast
        Debug.DrawRay(InteractionRaycastPosition(), transform.forward * rayLength, Color.yellow);
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return holdPoint;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
