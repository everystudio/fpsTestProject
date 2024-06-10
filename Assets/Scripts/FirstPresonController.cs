using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPresonController : MonoBehaviour
{
    public bool IsLookable { get; set; } = true;
    public bool IsMovable { get; set; } = true;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5.0f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Look")]
    [SerializeField, Range(1, 10)] private float lookSpeed = 2.0f;
    [SerializeField, Range(1, 100)] private float upperLookLimit = 50.0f;
    [SerializeField, Range(1, 100)] private float lowerLookLimit = 50.0f;

    private Camera playerCamera;
    private CharacterController characterController;

    [SerializeField] private Vector3 movement = Vector3.forward;
    private Vector2 currentInput = Vector2.zero;

    [SerializeField] private bool isGrounded = false;

    private void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (IsLookable)
        {
            Look();
        }

        if (IsMovable)
        {
            Move();
        }
    }

    private void Move()
    {
        currentInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 horizontalMovement = transform.TransformDirection(new Vector3(currentInput.x, 0, currentInput.y));
        horizontalMovement *= walkSpeed;

        Vector3 verticalMovement = Vector3.up * movement.y;
        if (!characterController.isGrounded)
        {
            verticalMovement.y += gravity * Time.deltaTime;
        }
        movement = horizontalMovement + verticalMovement;

        isGrounded = characterController.isGrounded;

        characterController.Move(movement * Time.deltaTime);
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
        /*
        euler = playerCamera.transform.localEulerAngles;
        euler.x -= delta.y * lookSpeed;
        euler.x = euler.x > 180 ? euler.x - 360 : euler.x;
        euler.x = Mathf.Clamp(euler.x, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localEulerAngles = euler;
        */
    }


}
