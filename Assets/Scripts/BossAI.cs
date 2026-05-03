using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    [Header("Target Settings")]
    public GameObject player; 

    [Header("Movement Settings")]
    public float stoppingDistance = 2.0f;
    public float attackRange = 3.0f; 

    [Header("Combat Settings")]
    public float attackCooldown = 2.0f; 
    private float lastAttackTime;

    private NavMeshAgent agent;
    private Animator anim;

    public GameObject hitParticles;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // Use GetComponentInChildren in case the Animator is on the 3D model child object
        anim = GetComponentInChildren<Animator>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (player == null) 
        {
            Debug.LogError("Boss cannot find the Player! Is the Toad tagged as 'Player'?");
        }

        if (agent != null)
        {
            agent.updateRotation = true;
            agent.angularSpeed = 120f;
            agent.stoppingDistance = stoppingDistance; 
        }
    }

    void Update()
    {
        if (player != null && agent != null && agent.isOnNavMesh)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            // 1. MOVEMENT LOGIC
            if (distanceToPlayer > stoppingDistance)
            {
                agent.isStopped = false;
                agent.SetDestination(player.transform.position);
            }
            else
            {
                agent.isStopped = true; 
                LookAtPlayer(); 
            }

            // 2. ATTACK TRIGGER LOGIC (The "Brain")
            if (distanceToPlayer <= attackRange)
            {
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    PerformAttack(); 
                }
            }

            // 3. ANIMATION SYNC
            if (anim != null)
            {
                // Tells the Animator how fast we are moving for Walk/Idle transitions
                anim.SetFloat("moveSpeed", agent.velocity.magnitude);
            }
            // Inside Update()
            if (distanceToPlayer > stoppingDistance)
                {
                    agent.isStopped = false;
                    
                    // Refresh the path every frame while chasing
                    agent.SetDestination(player.transform.position); 
                }
                else
                {
                    // If he's close enough, clear his path so he doesn't "creep" forward
                    agent.ResetPath(); 
                    agent.isStopped = true;
                    LookAtPlayer();
                }
        }
    }

    void LookAtPlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0; 
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    // This function STARTS the animation
    void PerformAttack()
    {
        lastAttackTime = Time.time;
        if (anim != null)
        {
            anim.SetTrigger("Attack"); 
            Debug.Log("Boss Brain: Starting Attack Animation");
        }
    }

    // This function is ONLY called by the Animation Event on the timeline// This MUST match the name in the Animation Event window exactly
public void DealDamage() 
{
    if (player != null)
    {
        // 1. Get the direction from Boss to Player
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        
        // 2. Check if the player is in FRONT of the boss
        float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);

        // 3. Measure distance
        float distance = Vector3.Distance(transform.position, player.transform.position);
        
        // Increase the buffer for the 2x scale
        // We add a 'size offset' because the Boss is thick!
        float bossSizeOffset = 1.5f; 

        if (distance <= (attackRange + bossSizeOffset) && dotProduct > 0) 
        {
            player.GetComponent<Health>()?.TakeDamage(25);
            Debug.Log("Boss landed a hit on the Toad Warrior!");
        }
    }
}
    public void TakeDamage(int damage)
    {
        if (hitParticles != null)
    {
        // Spawn the particles at the Boss's chest height
        Vector3 spawnPos = transform.position + Vector3.up * 1.0f; 
        Instantiate(hitParticles, spawnPos, Quaternion.identity);
    }
    
    {
        if (anim != null) anim.SetTrigger("GetHit");

        // Counter-Attack if player is close
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= attackRange)
        {
            PerformAttack(); 
        }
    }
}}