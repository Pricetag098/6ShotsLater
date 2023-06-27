using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour
{
    [SerializeField] float minVelocity = 0;
    [SerializeField] Optional<float> breakVelocity;
    [SerializeField] float minDamage;

    bool hitThisFrame;
    private void OnCollisionEnter(Collision collision)
    {
        float relativeVelocityMagnitude = collision.relativeVelocity.magnitude;
        if (breakVelocity.Enabled)
        {
            if(relativeVelocityMagnitude > breakVelocity.Value)
            {
                //break;
                DealDamage(collision,relativeVelocityMagnitude);
                return;
            }
        }
        else if(relativeVelocityMagnitude > minVelocity)
        {
            DealDamage(collision,relativeVelocityMagnitude);
        }
    }
    private void LateUpdate()
    {
        hitThisFrame = false;
    }

    void DealDamage(Collision collision,float velocity)
    {
        if(hitThisFrame)
            return;
        HitBox hitBox;
        if (collision.collider.TryGetComponent(out hitBox))
        {
            float hitDamage = minDamage;
            Debug.Log("Wack");
            hitBox.OnHit(hitDamage);
            hitThisFrame = true;
        }
    }
}
