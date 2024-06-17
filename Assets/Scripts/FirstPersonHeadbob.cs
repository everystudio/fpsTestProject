using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonHeadbob : MonoBehaviour
{
    public float swingSpeed = 3.5f;
    public float swingAmount = 0.025f;
    private float timer = 0.0f;

    public void Headbob(FirstPersonController firstPersonController, Transform cameraTransform, Vector3 velocity, float deltaTime)
    {
        if (!firstPersonController.IsGrounded)
        {
            return;
        }

        float velocityMagnitude = Vector3.Magnitude(new Vector3(velocity.x, 0f, velocity.z)); ;

        if (0.1f < velocityMagnitude)
        {
            float stateBonus = firstPersonController.IsSprinting ? 1.25f : firstPersonController.IsCrouching ? 0.5f : 1.0f;
            timer += deltaTime * velocityMagnitude * swingSpeed * stateBonus;
            cameraTransform.localPosition = new Vector3(
                cameraTransform.localPosition.x,
                Mathf.Sin(timer) * swingAmount * stateBonus,
                cameraTransform.localPosition.z);
        }

    }

}
