using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FirstPersonInteraction : MonoBehaviour
{
    [SerializeField] private Vector3 interactionRayOrigin = new Vector3(0.5f, 0.5f, 0.5f);
    [SerializeField] private float interactionRayDistance = 2.0f;
    [SerializeField] private LayerMask interactionLayerMask = default;
    [SerializeField] private KeyCode interactionKey = KeyCode.E;

    private IFirstPersonInteractable currentInteractable = null;
    public IFirstPersonInteractable CurrentInteractable => currentInteractable;
    public UnityEvent<IFirstPersonInteractable> OnInteractEvent = new UnityEvent<IFirstPersonInteractable>();
    public UnityEvent<IFirstPersonInteractable> OnInteractableIn = new UnityEvent<IFirstPersonInteractable>();
    public UnityEvent<IFirstPersonInteractable> OnInteractableOut = new UnityEvent<IFirstPersonInteractable>();

    public bool IsRequestInteract()
    {
        return Input.GetKeyDown(interactionKey);
    }

    public void HandleInteraction()
    {
        if (currentInteractable != null)
        {
            OnInteractEvent.Invoke(currentInteractable);
            currentInteractable.OnInteract();
        }
    }

    public void InteractionCheck(Camera camera)
    {
        Ray ray = camera.ViewportPointToRay(interactionRayOrigin);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactionRayDistance, interactionLayerMask))
        {
            IFirstPersonInteractable interactable = hit.collider.GetComponent<IFirstPersonInteractable>();
            if (interactable != null)
            {
                if (currentInteractable != interactable)
                {
                    if (currentInteractable != null)
                    {
                        currentInteractable.OnFocusOut();
                        OnInteractableOut.Invoke(currentInteractable);
                    }
                    currentInteractable = interactable;
                    currentInteractable.OnFocusIn();
                    OnInteractableIn.Invoke(currentInteractable);
                }
            }
            else
            {
                if (currentInteractable != null)
                {
                    currentInteractable.OnFocusOut();
                    OnInteractableOut.Invoke(currentInteractable);
                    currentInteractable = null;
                }
            }
        }
        else
        {
            if (currentInteractable != null)
            {
                currentInteractable.OnFocusOut();
                OnInteractableOut.Invoke(currentInteractable);
                currentInteractable = null;
            }
        }
    }
}
