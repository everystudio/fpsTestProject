using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonSlopeSlide : MonoBehaviour
{
    [SerializeField] private float slopeSpeed = 5.0f;
    public bool IsSliding(CharacterController characterController, out Vector3 slopeNomal)
    {
        slopeNomal = Vector3.zero;
        Vector3 raycastStart = characterController.transform.position + characterController.center;

        Debug.DrawRay(raycastStart, Vector3.down * (characterController.height / 2.0f + 1f), Color.red, 1.0f);

        if (characterController.isGrounded && Physics.Raycast(raycastStart, Vector3.down, out RaycastHit slopeHit, characterController.height / 2.0f + 1f))
        {
            slopeNomal = slopeHit.normal * slopeSpeed;
            return characterController.slopeLimit < Vector3.Angle(slopeNomal, Vector3.up);
        }
        else
        {
            return false;
        }
    }

}
