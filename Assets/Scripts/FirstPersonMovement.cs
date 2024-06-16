using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5.0f;
    [SerializeField] private float sprintSpeed = 10.0f;
    [SerializeField] private float crouchingSpeed = 1f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3.0f;

    public Vector3 verticalVelocity = Vector3.zero;

    private FirstPersonCrouch crouchComponent;

    private void Awake()
    {
        TryGetComponent(out crouchComponent);
    }

    public Vector3 Move(Vector2 currentInput, CharacterController characterController, float deltaTime, bool isJump, bool isSprint = false)
    {
        float movementSpeed = isSprint ? sprintSpeed : walkSpeed;
        // 座っている場合は上書き
        if (crouchComponent != null && crouchComponent.IsCrouching)
        {
            movementSpeed = crouchingSpeed;
        }

        Vector3 horizontalMovementVelocity = transform.TransformDirection(new Vector3(currentInput.x, 0, currentInput.y));
        horizontalMovementVelocity *= movementSpeed;

        if (isJump && characterController.isGrounded)
        {
            verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        if (!characterController.isGrounded)
        {
            verticalVelocity.y += gravity * deltaTime;
        }

        Vector3 velocity = horizontalMovementVelocity + verticalVelocity;


        characterController.Move(velocity * deltaTime);

        return velocity;
    }

}
