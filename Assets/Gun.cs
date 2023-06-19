using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class Gun : MonoBehaviour
{
    public ObjectPooler pooler;
    public float bulletSpeed;
    public Transform origin;
    // Start is called before the first frame update
    void Start()
    {
        pooler = GetComponent<ObjectPooler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(ActivateEventArgs eventArgs)
    {
        Debug.Log("Pew");
        GameObject bulletGo = pooler.SpawnObj();
        Bullet b = bulletGo.GetComponent<Bullet>();
        b.Shoot(transform.forward * bulletSpeed, origin.position);
    }
}
