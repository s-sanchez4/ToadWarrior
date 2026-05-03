using UnityEngine;
using UnityEngine.AI; 

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
            
            GetComponentInChildren<Animator>().SetTrigger("GetHit");
        }
    }
    

void Die()
{
    if (isDead) return;
    isDead = true;

    
    
    VictoryManager vm = FindFirstObjectByType<VictoryManager>();
    if (vm != null)
    {
        vm.BossDefeated(); // This actually triggers the count!
    }
    // -----------------------

   
    Animator anim = GetComponentInChildren<Animator>();
    if (anim != null) 
    {
        anim.Rebind(); 
        anim.SetTrigger("Die");
    }

  
    NavMeshAgent agent = GetComponentInChildren<NavMeshAgent>();
    if (agent == null) agent = GetComponentInParent<NavMeshAgent>();

    if (agent != null)
    {
        agent.isStopped = true; 
        agent.enabled = false;  
    }
 
   
    Rigidbody rb = GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.isKinematic = true; 
        rb.useGravity = false; 
    }

    
    Collider col = GetComponent<Collider>();
    if (col != null) 
    {
        col.isTrigger = true; 
    }

    
    if (GetComponent<BossAI>() != null) 
    {
        GetComponent<BossAI>().enabled = false;
    }

    Debug.Log(gameObject.name + " has been defeated!");
}}