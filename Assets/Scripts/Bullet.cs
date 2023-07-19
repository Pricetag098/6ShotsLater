using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;
    bool despawning = false;
    [SerializeField] float despawnDelay = 0.1f;
    public float damage;
    float timer;
    Collider col;
    MeshRenderer mr;
    //TrailRenderer tr;
    // Start is called before the first frame update

     public Optional<Grenade> grenade;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        mr = GetComponent<MeshRenderer>();
        //tr = GetComponent<TrailRenderer>();
    }
    private void Update()
    {
        if (despawning)
        {
            timer += Time.deltaTime;
            if(timer > despawnDelay)
            {
                Grenade g;
                if (TryGetComponent(out g))
                {
                    return;
                }
                //tr.time = 0;
                GetComponent<PooledObject>().Despawn();
            }
        }
        
    }

    public void Shoot(Vector3 vel,Vector3 origin,float dmg)
    {
        
        rb.velocity = vel;
        transform.position = origin;
        despawning = false;
        if (grenade.Enabled)
        {
            grenade.Value.Spawn();
            grenade.Value.ReleseTrigger();
        }
        else
        {
            col.enabled = true;
            mr.enabled = true;
            rb.isKinematic = false;
        }
        
        //tr.time = .1f;
        damage = dmg;
    }
	private void LateUpdate()
	{
        
        if (despawning)
		{
            if (grenade.Enabled)
            {
                
                //grenade.Value.Despawn();
            }
            else
            {
                col.enabled = false;
                mr.enabled = false;
                rb.isKinematic = true;
            }
            
        }
	}

	private void OnCollisionEnter(Collision collision)
    {
        
        if (despawning)
		{
            return;
        }


        //Debug.Log("Hit" + collision.collider.gameObject.name, collision.collider.gameObject);
        ContactPoint point = collision.GetContact(0);
        HitBox hitBox;
        if(collision.collider.TryGetComponent(out hitBox))
		{
            hitBox.OnHit(damage);
            VfxSpawner.SpawnVfx(0, point.point, point.normal);
		}
		else
		{
            VfxSpawner.SpawnVfx(1, point.point, point.normal);
        }


        despawning=true;
        
        
    }
}
