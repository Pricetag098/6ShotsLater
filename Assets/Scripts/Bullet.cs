using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;
    bool despawning = false;
    [SerializeField] float despawnDelay = 0.1f;
    float timer;
    Collider col;
    MeshRenderer mr;
    TrailRenderer tr;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        mr = GetComponent<MeshRenderer>();
        tr = GetComponent<TrailRenderer>();
    }
    private void Update()
    {
        if (despawning)
        {
            timer += Time.deltaTime;
            if(timer > despawnDelay)
            {
                tr.time = 0;
                GetComponent<PooledObj>().Despawn();
            }
        }
        
    }

    public void Shoot(Vector3 vel,Vector3 origin)
    {
        
        rb.velocity = vel;
        transform.position = origin;
        despawning = false;
        col.enabled = true;
        mr.enabled = true;
        rb.isKinematic = false;
        tr.time = .1f;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit" + collision.collider.gameObject.name, collision.collider.gameObject);
        
        despawning=true;
        col.enabled = false;
        mr.enabled = false;
        rb.isKinematic = true;
    }
}
