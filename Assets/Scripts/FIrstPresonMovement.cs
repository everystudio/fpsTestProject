using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FIrstPresonMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5.0f;
    [SerializeField] private float gravity = -9.81f;

    private Vector3 movement = Vector3.forward;

    public Vector3 Move(Vector2 currentInput, CharacterController characterController, float deltaTime)
    {
        Vector3 horizontalMovement = transform.TransformDirection(new Vector3(currentInput.x, 0, currentInput.y));
        horizontalMovement *= walkSpeed * deltaTime;

        Vector3 verticalMovement = Vector3.up * movement.y * deltaTime;
        if (!characterController.isGrounded)
        {
            verticalMovement.y += gravity * deltaTime;
        }
        movement = horizontalMovement + verticalMovement;

        return movement;
    }

}
