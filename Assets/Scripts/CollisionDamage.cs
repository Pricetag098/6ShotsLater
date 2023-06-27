using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour
{
    [SerializeField] float minVelocity = 0;
    [SerializeField] Optional<float> breakVelocity;
    [SerializeField] float minDamage;

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
        if(relativeVelocityMagnitude > minVelocity)
        {
            DealDamage(collision,relativeVelocityMagnitude);
        }
    }

    void DealDamage(Collision collision,float velocity)
    {
        
        HitBox hitBox;
        if (collision.collider.TryGetComponent(out hitBox))
        {
            float hitDamage = minDamage;
            
            hitBox.OnHit(hitDamage);
        }
    }
}
