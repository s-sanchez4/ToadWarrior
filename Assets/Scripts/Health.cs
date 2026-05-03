using UnityEngine;
using UnityEngine.AI; // Needed to disable the Boss's navigation
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float maxHealth = 150f;
    public float currentHealth;
    public Slider healthSlider; // Drag your ToadHealthBar here in the Inspector
   
   
    private bool isDead = false; // Prevents the Die logic from running multiple times

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
    
    // 1. Update the UI first so the player sees the bar drop
    if (healthSlider != null)
    {
        healthSlider.value = currentHealth;
    }

    Debug.Log($"{gameObject.name} took {amount} damage. Health: {currentHealth}");

    // 2. CHECK FOR DEATH
    if (currentHealth <= 0)
    {
        Die(); // This handles the "Die" trigger
    }
    else
    {
        // 3. ONLY HURT IF ALIVE
        // If we don't have this 'else', the Toad might play 
        // the Hurt animation instead of the Die animation.
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
 // Inside your Health.cs script
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

    // Disable scripts and physics
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