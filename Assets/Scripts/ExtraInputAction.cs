using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ExtraInputAction : MonoBehaviour
{
    [SerializeField] InputActionProperty reloadAction;
    [SerializeField] InputActionProperty shootAction;
    XRBaseInteractor interactor;
    // Start is called before the first frame update
    void Start()
    {
        reloadAction.action.performed += DoThing;
        
        interactor = GetComponent<XRBaseInteractor>();
    }
    private void OnEnable()
    {
        reloadAction.action.Enable();
        shootAction.action.Enable();
    }
    private void OnDisable()
    {
        reloadAction.action.Disable();
        shootAction.action.Disable();
    }
    private void OnDestroy()
    {
        reloadAction.action.performed -= DoThing;
    }

    private void Update()
	{
        IXRInteractable interactable = interactor.firstInteractableSelected;

        if (interactable != null)
        {
            Gun gun;
            if (interactable.transform.TryGetComponent(out gun))
            {
                gun.wasPressed = shootAction.action.IsPressed();
                gun.wasPressedThisFrame = shootAction.action.WasPressedThisFrame();
                //Debug.Log(shootAction.action.IsPressed());
            }
        }
    }

	void DoThing(InputAction.CallbackContext context)
    {
        //Debug.Log("Did thing");
        IXRInteractable interactable = interactor.firstInteractableSelected;
        
        if (interactor.hasSelection)
        {
            Gun gun;
            if(interactable.transform.TryGetComponent(out gun))
            {
                gun.StartReload();
            }
        }
    }
}
