using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDeathController : MonoBehaviour
{
    Health health;
    Animator animator;
    [SerializeField] string deathTrigger = "Death";
    
    ZombieAi zombieAi;
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        health.OnDeath += Die;
        
        animator = GetComponent<Animator>();  
        zombieAi = GetComponent<ZombieAi>();
    }

 

    void Die()
	{
        Debug.Log("Dead Zombie",gameObject);
        animator.SetTrigger(deathTrigger);
        zombieAi.Kill();
	}

    public void ResetZombie()
    {
        zombieAi.ResetZombie();
    }

}
