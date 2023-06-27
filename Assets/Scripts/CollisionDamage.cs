using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour
{
    [SerializeField] float minVelocity = 0;
    [SerializeField] Optional<float> breakVelocity;
    [SerializeField] float minDamage;
    [SerializeField] float cooldown =1;
    [SerializeField] Optional<SoundPlayer> hitSound;
    [SerializeField] Optional<SoundPlayer> breakSound;

    float timeOfLastHit;
    private void OnCollisionEnter(Collision collision)
    {
        if (Time.time - timeOfLastHit < cooldown)
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
                Destroy(gameObject);
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
        }
    }
    

    void DealDamage(Collision collision,float velocity)
    {
        timeOfLastHit = Time.time;
        HitBox hitBox;
        if (collision.collider.TryGetComponent(out hitBox))
        {
            float hitDamage = minDamage;
            Debug.Log("Wack");
            hitBox.OnHit(hitDamage);
            
        }
    }
}