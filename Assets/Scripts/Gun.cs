using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class Gun : MonoBehaviour
{
    ObjectPooler pooler;
    [SerializeField] float bulletSpeed = 100,bulletSpreadDegrees = 0;
    [SerializeField] int shotsPerFiring = 1;
    [SerializeField] int ammoLeft, maxAmmo = 10;
    [SerializeField] float fireCoolDown = 1;
    float fireTimer = 0;
    [SerializeField] Transform origin;
    // Start is called before the first frame update
    void Start()
    {
        pooler = GetComponent<ObjectPooler>();
        ammoLeft = maxAmmo;
    }
    private void Update()
    {
        fireTimer -= Time.deltaTime;
    }

    public void Shoot(ActivateEventArgs eventArgs)
    {
        if(fireTimer <= 0)
        {
            if (ammoLeft > 0)
            {
                for (int i = 0; i < shotsPerFiring; i++)
                {
                    GameObject bulletGo = pooler.SpawnObj();
                    Bullet b = bulletGo.GetComponent<Bullet>();

                    Vector2 randVal = Random.insideUnitCircle * bulletSpreadDegrees;
                    Vector3 dir = Quaternion.Euler(randVal.x, randVal.y, 0) * transform.forward;
                    b.Shoot(dir * bulletSpeed, origin.position);
                }
                ammoLeft--;
                fireTimer = fireCoolDown;

                //play gun sound / fx
            }
            //play no ammo sound / fx
        }
        
        

        
    }
}
