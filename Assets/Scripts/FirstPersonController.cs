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

    [Header("Look")]
    [SerializeField, Range(1, 10)] private float lookSpeed = 2.0f;
    [SerializeField, Range(1, 100)] private float upperLookLimit = 50.0f;
    [SerializeField, Range(1, 100)] private float lowerLookLimit = 50.0f;

    private Camera playerCamera;
    private CharacterController characterController;

    private Vector2 currentInput = Vector2.zero;

    [SerializeField] private bool isGrounded = false;

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
                playerCamera.transform, playerCamera.transform.localPosition.y, 0.5f,
                characterController.height, 1.0f, characterController.center, new Vector3(0, 0.5f, 0));
        }
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
            movementComponent?.Move(
                currentInput, characterController, Time.deltaTime,
                Input.GetKeyDown(KeyCode.Space),
                Input.GetKey(KeyCode.LeftShift));
            isGrounded = characterController.isGrounded;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            crouchComponent?.ToggleCrouch(characterController, 0.5f);
        }
    }

    private void Look()
    {
        Vector2 delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 euler = transform.eulerAngles;

        euler.y += delta.x * lookSpeed;
        transform.eulerAngles = euler;

        float cameraEulerX = playerCamera.transform.localEulerAngles.x;
        cameraEulerX -= delta.y * lookSpeed;
        cameraEulerX = 180 < cameraEulerX ? cameraEulerX - 360 : cameraEulerX;
        cameraEulerX = Mathf.Clamp(cameraEulerX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(cameraEulerX, 0, 0);
    }
}
