using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class FirstPersonCrouch : MonoBehaviour
{
    public bool IsCrouching => isCrouching;
    private bool isCrouching = false;

    private Transform cameraRootTransform;
    private float standingCameraHeight;
    private float crouchingCameraHeight;
    private float standingHeight = 2.0f;
    private float crouchingHeight = 1.0f;
    private Vector3 standingCenter;
    private Vector3 crouchingCenter;

    public void Initialize(Transform cameraRootTransform, float standingCameraHeight, float crouchingCameraHeight, float standingHeight, float crouchingHeight, Vector3 standingCenter, Vector3 crouchingCenter)
    {
        this.cameraRootTransform = cameraRootTransform;
        this.standingCameraHeight = standingCameraHeight;
        this.crouchingCameraHeight = crouchingCameraHeight;
        this.standingHeight = standingHeight;
        this.crouchingHeight = crouchingHeight;
        this.standingCenter = standingCenter;
        this.crouchingCenter = crouchingCenter;
    }

    public async void ToggleCrouch(CharacterController characterController, float duration)
    {
        if (isCrouching)
        {
            Vector3 raycastStart = characterController.transform.position + characterController.center;
            //Debug.DrawLine(raycastStart, raycastStart + Vector3.up * crouchingHeight, Color.red, 1.0f);
            if (Physics.Raycast(raycastStart, Vector3.up, out RaycastHit hit, crouchingHeight))
            {
                // 上に何かあるので立てない
                return;
            }
        }
        isCrouching = !isCrouching;

        float currentHeight = characterController.height;
        float targetHeight = isCrouching ? crouchingHeight : standingHeight;
        Vector3 currentCenter = characterController.center;
        Vector3 targetCenter = isCrouching ? crouchingCenter : standingCenter;
        float currentCameraHeight = cameraRootTransform.localPosition.y;
        float targetCameraHeight = isCrouching ? crouchingCameraHeight : standingCameraHeight;

        float time = 0;
        while (time < duration)
        {
            float cameraHeight = Mathf.Lerp(currentCameraHeight, targetCameraHeight, time / duration);
            cameraRootTransform.localPosition = new Vector3(cameraRootTransform.localPosition.x, cameraHeight, cameraRootTransform.localPosition.z);

            characterController.height = Mathf.Lerp(currentHeight, targetHeight, time / duration);
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, time / duration);

            time += Time.deltaTime;
            await Task.Delay((int)(Time.deltaTime * 1000));
        }

    }


}
