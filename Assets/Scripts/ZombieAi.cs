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
    Health health;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.position);
        if(agent.remainingDistance < attackDistance)
        {
            animator.SetTrigger(attackTrigger);
        }
    }
    public void Reset()
    {
        animator.SetTrigger(reset);
        health.health = health.maxHealth;

    }
    public void DealDamage()
    {

    }
}
