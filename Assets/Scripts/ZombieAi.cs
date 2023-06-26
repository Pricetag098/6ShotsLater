using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class ZombieAi : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] Transform target;
    [SerializeField] float attackDistance = 1;
    Animator animator;
    [SerializeField] string attackTrigger = "Attack";
    [SerializeField] string reset = "Reset";
    [SerializeField] string velocity = "Velocity";
    [SerializeField] float damagePerHit;
    Health health;
    Health playerHealth;
    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position);
        if(Vector3.Distance(target.position,transform.position) < attackDistance)
        {
            animator.SetTrigger(attackTrigger);
        }
        animator.SetFloat(velocity, agent.velocity.magnitude);
    }
    public void ResetZombie()
    {
        animator.SetTrigger(reset);
        health.health = health.maxHealth;
        agent.enabled = false;
        GetComponent<PooledObject>().Despawn();

    }
    public void DealDamage()
    {
        playerHealth.TakeDmg(damagePerHit);
    }

    public void Spawn(Transform player, Health pHealth)
    {
        target = player;
        playerHealth = pHealth;
        agent.enabled = true;
    }
}
