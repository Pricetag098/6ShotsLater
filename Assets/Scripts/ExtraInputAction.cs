using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ExtraInputAction : MonoBehaviour
{
    [SerializeField]InputActionProperty inputActions;
    XRBaseInteractor interactor;
    // Start is called before the first frame update
    void Start()
    {
        inputActions.action.performed += DoThing;
        interactor = GetComponent<XRBaseInteractor>();
    }
    private void OnEnable()
    {
        inputActions.action.Enable();
    }
    private void OnDisable()
    {
        inputActions.action.Disable();
    }

    
    void DoThing(InputAction.CallbackContext context)
    {
        Debug.Log("Did thing");
        IXRInteractable interactable = interactor.firstInteractableSelected;
        if (interactable != null)
        {
            Gun gun = interactable.transform.GetComponent<Gun>();
            if(gun != null)
            {
                gun.StartReload();
            }
        }
    }
}
