using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
public class RangedGrab : MonoBehaviour
{
    [SerializeField] InputActionProperty inputActions;

    bool selected = false;
    [SerializeField] float flightTime = 1;
    [SerializeField] float pullDistance = 0.1f;
    float time =0;
    Vector3 origin, anchor;
    Quaternion originalAngles;
    Rigidbody target;
    ActionBasedController controller;
    XRBaseInteractor interactor;

    Vector3 lastHandPos;
    private void OnEnable()
    {
        inputActions.action.Enable();
    }
    private void OnDisable()
    {
        inputActions.action.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        inputActions.action.performed += TryGrab;
        controller = GetComponent<ActionBasedController>();
        interactor = GetComponent<XRBaseInteractor>();
    }

    void TryGrab(InputAction.CallbackContext context)
	{
        if (interactor.firstInteractableSelected != null || selected)
            return;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit))
		{
            XRGrabInteractable interactable; ;
            if(hit.collider.TryGetComponent(out interactable))
			{
                
                controller.SendHapticImpulse(.5f, 0.1f);
                target = interactable.GetComponent<Rigidbody>();
                
            }
            else if(hit.collider.transform.parent != null)
			{

                if (hit.collider.transform.parent.TryGetComponent(out interactable))
                {

                    controller.SendHapticImpulse(.5f, 0.1f);
                    target = interactable.GetComponent<Rigidbody>();

                }
            }

		}
        
	}
    void EndGrabAttempt()
	{
        
        target = null;
    }
    
    void StartGrab()
	{
        controller.SendHapticImpulse(.5f, 0.1f);
        selected = true;
        target.isKinematic = true;
        origin = target.transform.position;
        anchor = Vector3.Lerp(transform.position, origin, 0.5f);
        anchor.y = Mathf.Max(transform.position.y, origin.y) + 1;
        originalAngles = target.transform.rotation;
    }
    void EndGrab()
    {
        selected = false;
        target.isKinematic = false;
        target = null;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (target!=null && interactor.firstInteractableSelected != null)
		{
            //Debug.Log("AAAAA");
            EndGrab();
		}
        else if (target != null)
		{
            if(!selected)
		    {
                if (inputActions.action.WasReleasedThisFrame())
                {
                    EndGrabAttempt();
                }
                else
                {
                    if (Vector3.Distance(lastHandPos, transform.position) > pullDistance * Time.deltaTime && Vector3.Dot((transform.position - lastHandPos) ,target.position - transform.position ) < 0)
                    {
                        StartGrab();
                    }
                }
            }
            else
            {
                time += Time.deltaTime;
                target.transform.position = NotBezierLerp(origin, anchor, transform.position, time / flightTime);
                target.transform.rotation = Quaternion.Slerp(originalAngles,transform.rotation, time / flightTime);
                if (time > flightTime)
                {
                    EndGrab();
                }
            }
        }

        lastHandPos = transform.position;
    }

    

    Vector3 NotBezierLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        t = Mathf.Clamp01(t);
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(ab, bc, t);
    }
    Vector3 BezierLerp(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        Vector3 ad = NotBezierLerp(a, c, d, t);
        Vector3 cb = NotBezierLerp(c, d, b, t);
        return Vector3.Lerp(ad, cb, t);
    }
}
