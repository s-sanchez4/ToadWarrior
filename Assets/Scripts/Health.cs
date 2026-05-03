using UnityEngine;
using UnityEngine.AI; 
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float maxHealth = 150f;
    public float currentHealth;
    public Slider healthSlider; // Drag your ToadHealthBar here in the Inspector
   
   
    private bool isDead = false; 

    private Animator anim;
    

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponentInChildren<Animator>();
    }

  public void TakeDamage(float amount)
{
    if (isDead) return; 

    currentHealth -= amount;
    
    
    if (healthSlider != null)
    {
        healthSlider.value = currentHealth;
    }

    Debug.Log($"{gameObject.name} took {amount} damage. Health: {currentHealth}");

    
    if (currentHealth <= 0)
    {
        Die(); 
    }
    else
    {
        
        if (anim != null) anim.SetTrigger("Hurt");
    }
}

    public void Heal(int amount)
    {
        if (isDead) return;
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (healthSlider != null) 
    {
        healthSlider.value = currentHealth;
    }
        Debug.Log("Health Healed: " + currentHealth);
    }
 
void Die()
{
    if (isDead) return;
    isDead = true;

    Animator playerAnim = GetComponentInChildren<Animator>();
    if (playerAnim != null)
    {
        // 1. Reset 'Hurt' so it doesn't override the death
        playerAnim.ResetTrigger("Hurt");

        // 2. Stop the Blend Tree movement
        playerAnim.SetFloat("MoveX", 0);
        playerAnim.SetFloat("MoveZ", 0);
        playerAnim.SetFloat("moveSpeed", 0);

        // 3. Fire the trigger
        playerAnim.SetTrigger("Die");
    }

    
    PlayerController moveScript = GetComponent<PlayerController>();
    if (moveScript != null) moveScript.enabled = false;

    Rigidbody rb = GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true; 
    }

    Debug.Log("Toad Warrior is playing Death Animation!");
}}