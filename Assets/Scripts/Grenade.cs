using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] float timer;
    float time;
    public bool cooking;
    [SerializeField] LayerMask targetLayers;
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionForce;
    [SerializeField] float explosionDamage;
    [SerializeField] ParticleSystem particle;
    [SerializeField] SoundPlayer explodeSound;
    [SerializeField] SoundPlayer cookSound;

   
    public void ReleseTrigger()
    {
        if (!cooking)
        {
            cooking = true;
            cookSound.Play();
        }
        
    }
    private void Update()
    {
        if (cooking)
        {
            time += Time.deltaTime;
            if(time > timer)
            {
                Explode();
            }
        }
    }

    void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, targetLayers);
        for(int i = 0; i < hits.Length; i++)
        {
            Rigidbody rb;
            if (TryGetComponent(out rb))
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
            HitBox hb;
            if(TryGetComponent(out hb))
            {
                hb.OnHit(explosionDamage/ Mathf.Pow(Vector3.Distance(transform.position,hb.transform.position),2));
            }
        }
        particle.Play();
        explodeSound.Play();
        enabled = false;
    }
}
