using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    public bool IsLookable { get; set; } = true;
    public bool IsMovable { get; set; } = true;

    private FirstPersonMovement movementComponent;
    private FirstPersonCrouch crouchComponent;
    private FirstPersonHeadbob headbobComponent;
    private FirstPersonZoom zoomComponent;

    [Header("Look")]
    [SerializeField, Range(1, 10)] private float lookSpeed = 2.0f;
    [SerializeField, Range(1, 100)] private float upperLookLimit = 50.0f;
    [SerializeField, Range(1, 100)] private float lowerLookLimit = 50.0f;

    [SerializeField] private Transform cameraRoot;
    private Camera playerCamera;
    private CharacterController characterController;

    private Vector2 currentInput = Vector2.zero;


    [SerializeField] private bool isGrounded = false;
    public bool IsGrounded => isGrounded;

    public bool IsSprinting
    {
        get
        {
            if (movementComponent != null)
            {
                return movementComponent.IsSprinting;
            }
            return false;
        }
    }
    public bool IsCrouching
    {
        get
        {
            if (crouchComponent != null)
            {
                return crouchComponent.IsCrouching;
            }
            return false;
        }
    }

    private void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        movementComponent = GetComponent<FirstPersonMovement>();
        if (TryGetComponent(out crouchComponent))
        {
            crouchComponent.Initialize(
                cameraRoot, cameraRoot.localPosition.y, 0.5f,
                characterController.height, 1.0f, characterController.center, new Vector3(0, 0.5f, 0));
        }
        TryGetComponent(out headbobComponent);
        TryGetComponent(out zoomComponent);
    }

    private void Update()
    {
        if (IsLookable)
        {
            Look();
        }

        if (IsMovable)
        {
            currentInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            Vector3 moveVelocity = movementComponent.Move(
                currentInput, characterController, Time.deltaTime,
                Input.GetKeyDown(KeyCode.Space),
                Input.GetKey(KeyCode.LeftShift));
            isGrounded = characterController.isGrounded;
            headbobComponent?.Headbob(this, playerCamera.transform, moveVelocity, Time.deltaTime);

        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            crouchComponent?.ToggleCrouch(characterController, 0.5f);
        }

        if (zoomComponent != null && zoomComponent.ZoomRequest())
        {
            zoomComponent.ToggleZoom();
        }
    }

    private void Look()
    {
        Vector2 delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 euler = transform.eulerAngles;

        euler.y += delta.x * lookSpeed;
        transform.eulerAngles = euler;

        float cameraEulerX = cameraRoot.localEulerAngles.x;
        cameraEulerX -= delta.y * lookSpeed;
        cameraEulerX = 180 < cameraEulerX ? cameraEulerX - 360 : cameraEulerX;
        cameraEulerX = Mathf.Clamp(cameraEulerX, -upperLookLimit, lowerLookLimit);
        cameraRoot.localRotation = Quaternion.Euler(cameraEulerX, 0, 0);
    }
}
