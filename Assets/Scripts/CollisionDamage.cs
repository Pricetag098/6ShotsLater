using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CollisionDamage : MonoBehaviour
{
    [SerializeField] float minVelocity = 0;
    [SerializeField] Optional<float> breakVelocity;
    [SerializeField] float minDamage;
    [SerializeField] float cooldown =1;
    [SerializeField] Optional<SoundPlayer> hitSound;
    [SerializeField] Optional<SoundPlayer> breakSound;
    [SerializeField] bool dropOnBreak = true;
    XRGrabInteractable interactable;
    [SerializeField] float breakHapticDuration = .1f;
    [Range(0, 1)][SerializeField] float breakHapticStrength = 1;
    [SerializeField] float hitHapticDuration = .1f;
    [Range(0, 1)][SerializeField] float hitHapticStrength = 1;
    bool hit = false;
    private void Start()
    {
        interactable = GetComponent<XRGrabInteractable>();
    }


    float timeOfLastHit;
    private void OnCollisionEnter(Collision collision)
    {
        //if (Time.time - timeOfLastHit < cooldown)
        //    return;
        if (hit)
            return;
        float relativeVelocityMagnitude = collision.relativeVelocity.magnitude;
        if (breakVelocity.Enabled)
        {
            if(relativeVelocityMagnitude > breakVelocity.Value)
            {
                //break;
                DealDamage(collision,relativeVelocityMagnitude);
                if (breakSound.Enabled)
                {
                    breakSound.Value.Play();
                }
                //Destroy(gameObject);
                if(dropOnBreak)
                interactable.enabled = false;
                if(interactable.firstInteractorSelecting != null)
                    interactable.firstInteractorSelecting.transform.GetComponent<XRController>().SendHapticImpulse(breakHapticStrength,breakHapticDuration);
                return;
            }
        }
        else if(relativeVelocityMagnitude > minVelocity)
        {
            DealDamage(collision,relativeVelocityMagnitude);
            if (hitSound.Enabled)
            {
                hitSound.Value.Play();
            }
            if (interactable.firstInteractorSelecting != null)
                interactable.firstInteractorSelecting.transform.GetComponent<XRController>().SendHapticImpulse(hitHapticStrength, hitHapticDuration);
        }
    }
    

    void DealDamage(Collision collision,float velocity)
    {
        hit = true;
        timeOfLastHit = Time.time;
        HitBox hitBox;
        if (collision.collider.TryGetComponent(out hitBox))
        {
            float hitDamage = minDamage;
            Debug.Log("Wack");
            hitBox.OnHit(hitDamage);
            
            
        }
    }

	private void OnCollisionExit(Collision collision)
	{
		hit= false;
	}
}
