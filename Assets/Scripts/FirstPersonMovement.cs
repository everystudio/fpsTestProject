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

    public bool IsSprinting { get; set; } = false;

    public Vector3 verticalVelocity = Vector3.zero;

    private FirstPersonCrouch crouchComponent;
    private FirstPersonSlopeSlide slopeSlideComponent;

    private void Awake()
    {
        TryGetComponent(out crouchComponent);
        TryGetComponent(out slopeSlideComponent);
    }

    public Vector3 Move(Vector2 currentInput, CharacterController characterController, float deltaTime, bool isJump, bool isSprint = false)
    {
        IsSprinting = isSprint;
        float movementSpeed = IsSprinting ? sprintSpeed : walkSpeed;
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
        else if (!characterController.isGrounded)
        {
            verticalVelocity.y += gravity * deltaTime;
        }
        else
        {
            verticalVelocity.y = gravity * 0.1f;
        }
        // stashテスト

        Vector3 velocity = Vector3.zero;
        if (slopeSlideComponent != null && slopeSlideComponent.IsSliding(characterController, out Vector3 slopeNormal))
        {
            verticalVelocity = Vector3.zero;
            horizontalMovementVelocity *= 0.3f;
            velocity += new Vector3(slopeNormal.x, -slopeNormal.y, slopeNormal.z);
        }
        velocity += horizontalMovementVelocity + verticalVelocity;


        characterController.Move(velocity * deltaTime);

        return velocity;
    }

}
