using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class FirstPersonHeadbob : MonoBehaviour
{
    public float swingSpeed = 200f;
    public float swingAmount = 0.025f;
    private float degree = 0.0f;


    [HideInInspector]
    public UnityEvent OnFootStep = new UnityEvent();

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
            float currentDegree = Mathf.Repeat(degree + deltaTime * velocityMagnitude * swingSpeed * stateBonus, 360.0f);
            float currentRadian = currentDegree * Mathf.Deg2Rad;

            if (degree < 270f && 270f <= currentDegree)
            {
                OnFootStep.Invoke();
            }
            degree = currentDegree;

            cameraTransform.localPosition = new Vector3(
                cameraTransform.localPosition.x,
                Mathf.Sin(currentRadian) * swingAmount * stateBonus,
                cameraTransform.localPosition.z);
        }

    }

}
