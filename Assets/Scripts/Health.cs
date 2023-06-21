using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health;
    public float maxHealth = 100;

    public delegate void DieAction();
    public DieAction OnDeath;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDmg(float dmg)
    {
        health-=dmg;
        if(health <= 0)
		{
            Die();
		}
    }

    void Die()
	{
        //do die stuff
        Debug.Log("dead",gameObject);
        OnDeath();
	}
}
