using UnityEngine;
using UnityEngine.AI; // Needed to talk to the NavMeshAgent

public class EnemyHealth : MonoBehaviour
{
    public int health = 100;
    private bool isDead = false; // Prevents the death logic from running twice

    public void TakeDamage(int damage)
    {
        if (isDead) return; // If he's already dead, ignore further hits

        health -= damage;
        
        if (health <= 0)
        {
            Die();
        }
        else
        {
            // Play the hit animation we set up earlier
            GetComponentInChildren<Animator>().SetTrigger("GetHit");
        }
    }
    

void Die()
{
    if (isDead) return;
    isDead = true;

    // --- THE FIX IS HERE ---
    // 1. Tell the VictoryManager a boss has died
    VictoryManager vm = FindFirstObjectByType<VictoryManager>();
    if (vm != null)
    {
        vm.BossDefeated(); // This actually triggers the count!
    }
    // -----------------------

    // 2. Animator Search
    Animator anim = GetComponentInChildren<Animator>();
    if (anim != null) 
    {
        anim.Rebind(); 
        anim.SetTrigger("Die");
    }

    // 3. Agent Search
    NavMeshAgent agent = GetComponentInChildren<NavMeshAgent>();
    if (agent == null) agent = GetComponentInParent<NavMeshAgent>();

    if (agent != null)
    {
        agent.isStopped = true; 
        agent.enabled = false;  
    }
 
    // 4. Physics Fix
    Rigidbody rb = GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.isKinematic = true; 
        rb.useGravity = false; 
    }

    // 5. Collider Fix
    Collider col = GetComponent<Collider>();
    if (col != null) 
    {
        col.isTrigger = true; 
    }

    // 6. Disable the AI script
    if (GetComponent<BossAI>() != null) 
    {
        GetComponent<BossAI>().enabled = false;
    }

    Debug.Log(gameObject.name + " has been defeated!");
}}