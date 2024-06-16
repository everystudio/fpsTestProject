using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5.0f;
    [SerializeField] private float sprintSpeed = 10.0f;
    [SerializeField] private float gravity = -9.81f;

    private Vector3 movement = Vector3.forward;

    public Vector3 Move(Vector2 currentInput, CharacterController characterController, float deltaTime, bool isSprint = false)
    {
        float movementSpeed = isSprint ? sprintSpeed : walkSpeed;
        Vector3 horizontalMovement = transform.TransformDirection(new Vector3(currentInput.x, 0, currentInput.y));
        horizontalMovement *= movementSpeed * deltaTime;

        Vector3 verticalMovement = Vector3.up * movement.y * deltaTime;
        if (!characterController.isGrounded)
        {
            verticalMovement.y += gravity * deltaTime;
        }
        movement = horizontalMovement + verticalMovement;

        return movement;
    }

}
