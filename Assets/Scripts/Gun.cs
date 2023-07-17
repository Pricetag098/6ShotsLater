using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
public class Gun : MonoBehaviour
{
    [SerializeField] bool auto;
    [SerializeField] float damage = 10;
    ObjectPooler pooler;
    [SerializeField] float bulletSpeed = 100,bulletSpreadDegrees = 0;
    [SerializeField] int shotsPerFiring = 1;
    [SerializeField] int ammoLeft, maxAmmo = 10;
    [SerializeField] float fireCoolDown = 1,reloadDuration = 1;
    float fireTimer = 0,reloadTimer;
    bool reloading = false;
    [SerializeField] Transform origin;
    Rigidbody body;
    [SerializeField]Vector3 recoilForce;
    XRGrabInteractable interactable;
    [SerializeField] float shootHapticDuration = .1f;
    [Range(0,1)] [SerializeField] float shootHapticStrength = 1;
    [SerializeField] float reloadHapticDuration = .1f;
    [Range(0, 1)][SerializeField] float reloadHapticStrength = 1;
    [SerializeField] SoundPlayer shootSound, reloadSound, emptySound;
    [SerializeField] Optional<ParticleSystem> gunfire;


    [HideInInspector]public bool wasPressed;
    [HideInInspector]public bool wasPressedThisFrame;
    // Start is called before the first frame update
    void Start()
    {
        pooler = GetComponent<ObjectPooler>();
        body = GetComponent<Rigidbody>();
        interactable = GetComponent<XRGrabInteractable>();
        ammoLeft = maxAmmo;
    }
    private void Update()
    {
        fireTimer -= Time.deltaTime;
        if (reloading)
        {
            reloadTimer += Time.deltaTime;
            if(reloadTimer >= reloadDuration)
            {
                Reload();
            }
        }
        if ((wasPressedThisFrame && !auto)|| (wasPressed && auto))
            Shoot();
    }

    public void Shoot()
    {
        if(fireTimer <= 0 && !reloading)
        {
            if (ammoLeft > 0)
            {
                for (int i = 0; i < shotsPerFiring; i++)
                {
                    GameObject bulletGo = pooler.Spawn();
                    Bullet b = bulletGo.GetComponent<Bullet>();

                    Vector2 randVal = Random.insideUnitCircle * bulletSpreadDegrees;
                    Vector3 dir = Quaternion.Euler(randVal.x, randVal.y, 0) * transform.forward;
                    b.Shoot(dir * bulletSpeed, origin.position, damage);
                    Grenade g;
                    if(b.TryGetComponent(out g))
                    {
                        g.ReleseTrigger();
                    }
                }
                ammoLeft--;
                fireTimer = fireCoolDown;
                interactable.firstInteractorSelecting.transform.GetComponent<ActionBasedController>().SendHapticImpulse(shootHapticStrength,shootHapticDuration);
                //.transform.GetComponent<XRController>().SendHapticImpulse(1, 0.1f);
                body.AddForce(transform.up * recoilForce.y + transform.forward * recoilForce.z + transform.right * recoilForce.x);
                //play gun sound / fx
                shootSound.Play();
                if (gunfire.Enabled)
                {
                    gunfire.Value.Play();
                }
            }
            //play no ammo sound / fx
            emptySound.Play();
        }

    }
    public void OnDrop()
	{

	}


    public void StartReload()
    {
        if (reloading || ammoLeft == maxAmmo)
            return;
        reloading = true;
        reloadTimer = 0;
        interactable.firstInteractorSelecting.transform.GetComponent<ActionBasedController>().SendHapticImpulse(reloadHapticStrength, reloadHapticDuration);
        reloadSound.Play();
    }
    void Reload()
    {
        ammoLeft = maxAmmo;
        reloading = false;
        // pee pee poo poo
    }
}
