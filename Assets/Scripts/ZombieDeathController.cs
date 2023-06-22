using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDeathController : MonoBehaviour
{
    Health health;
    Animator animator;
    [SerializeField] string Deathtrigger = "Deatht";
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        health.OnDeath += Die;
        animator = GetComponent<Animator>();    
    }

    void Die()
	{
        Debug.Log("Dead Zombie",gameObject);
        animator.SetTrigger(Deathtrigger);
	}

}
