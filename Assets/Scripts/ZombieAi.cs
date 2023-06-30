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
    [SerializeField] string hitTrigger = "Impact";
    bool recoiling = false;
    public bool dead = false;
    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        health.OnHit += TakeDmg;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.enabled)
        {
            if (!recoiling)
            {
                agent.isStopped = false;
                agent.SetDestination(target.position);
                Vector2 zombiePlanePos = new Vector2(transform.position.x, transform.position.z);
                Vector2 targetPlanePos = new Vector2(target.position.x, target.position.z);
                if (Vector2.Distance(zombiePlanePos, targetPlanePos) < attackDistance)
                {
                    animator.SetTrigger(attackTrigger);
                }
                animator.SetFloat(velocity, agent.velocity.magnitude);
            }
            else
            {
                agent.isStopped = true;
            }
            
        }
        
    }

    [ContextMenu("Kill")]
    public void KillInstant()
	{
        health.TakeDmg(1000000000000000000);
	}



    void TakeDmg()
    {
        if(!recoiling)
            animator.SetTrigger(hitTrigger);
        recoiling = true;
    }

    public void Kill()
    {
        agent.enabled = false;
    }
    public void ResetZombie()
    {
        animator.SetTrigger(reset);
        health.health = health.maxHealth;
        dead = true;
        GetComponent<PooledObject>().Despawn();

    }
    public void DealDamage()
    {
        Vector2 zombiePlanePos = new Vector2(transform.position.x, transform.position.z);
        Vector2 targetPlanePos = new Vector2(target.position.x, target.position.z);
        if (Vector2.Distance(zombiePlanePos, targetPlanePos) < attackDistance)
        {
            playerHealth.TakeDmg(damagePerHit);
        }
        Debug.Log("dealingDamage");
    }

    public void Spawn(Transform player, Health pHealth, float moveSpeed)
    {
        target = player;
        playerHealth = pHealth;
        agent.enabled = true;
        agent.isStopped = false;
        recoiling = false;
        dead = false;
        agent.speed = moveSpeed;
    }

    public void StopRecoiling()
    {
        recoiling = false;
    }
}
