using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Grenade : MonoBehaviour
{
    [SerializeField] float timer;
    float time;
    public bool cooking;
    [SerializeField] LayerMask targetLayers;
    [SerializeField] float explosionRadius;
    [SerializeField] float explosionForce;
    [SerializeField] float explosionDamage;
    [SerializeField] Optional<ParticleSystem> particle;
    [SerializeField] SoundPlayer explodeSound;
    [SerializeField] SoundPlayer cookSound;
    [SerializeField] GameObject body;
    [SerializeField] bool explodeOnImpact;
   
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
        if (cooking && !explodeOnImpact)
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
            if (hits[i].TryGetComponent(out rb))
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
            HitBox hb;
            if(hits[i].TryGetComponent(out hb))
            {
                hb.OnHit(explosionDamage/ Mathf.Pow(Vector3.Distance(transform.position,hb.transform.position),2));
            }
        }
        if(particle.Enabled)
        particle.Value.Play();
        explodeSound.Play();
        body.SetActive(false);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<XRGrabInteractable>().enabled = false;
        enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (explodeOnImpact)
            Explode();
    }
}
