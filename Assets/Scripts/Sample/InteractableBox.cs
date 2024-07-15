using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBox : MonoBehaviour, IFirstPersonInteractable
{
    public void OnFocusIn()
    {
        Debug.Log("InteractableBox: OnFocusIn");
    }

    public void OnFocusOut()
    {
        Debug.Log("InteractableBox: OnFocusOut");
    }

    public void OnInteract()
    {
        Debug.Log("InteractableBox: OnInteract");
    }
}
