using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Shoot(Vector3 vel,Vector3 origin)
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = vel;
        transform.position = origin;
    }
    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<PooledObj>().Despawn();
    }
}
