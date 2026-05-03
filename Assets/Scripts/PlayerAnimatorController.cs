using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject hitEffectPrefab; // Drag your particle prefab here in the Inspector
    private Animator animator;
    public float moveSpeed = 10f; 
    public float rotationSpeed = 10f;
    public float walkSpeed = 10f;
public float runSpeed = 1f;
private float currentSpeed = 5f;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        
        if (animator == null)
            Debug.LogError("Animator not found on " + gameObject.name + "! Please attach an Animator to the model.");
    }
   void Update()
{
    if (animator == null) return;

    // 1. INPUT
    float h = Input.GetAxis("Horizontal");
    float v = Input.GetAxis("Vertical");
    
    // 2. SPRINT LOGIC
    // If holding Left Shift, we multiply the input by 2 to reach the "Run" zones in the Blend Tree
    bool isRunning = Input.GetKey(KeyCode.LeftShift);
    float speedMultiplier = isRunning ? 2.0f : 1.0f;
    currentSpeed = isRunning ? runSpeed : walkSpeed;

    Vector3 moveDir = new Vector3(h, 0, v);
    
    if (moveDir.magnitude > 0.1f)
    {
        // Move the Toad
        transform.Translate(moveDir.normalized * currentSpeed * Time.deltaTime, Space.World);
        
        // --- UPDATED ROTATION ---
        Quaternion targetRot = Quaternion.LookRotation(moveDir);
        
        // Instead of Slerp, we use RotateTowards for a "Fixed" speed turn.
        // Try setting rotationSpeed to 720 in the Inspector for a "snappy" feel.
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
    }
    // 3. ANIMATOR SYNC
    // Multiplying h and v by speedMultiplier pushes the values to -2 or 2
    animator.SetFloat("MoveX", h * speedMultiplier);
    animator.SetFloat("MoveZ", v * speedMultiplier);
    animator.SetFloat("moveSpeed", moveDir.magnitude * speedMultiplier);


        // --- 2. ACTIONS (TRIGGERS) ---
        if (Input.GetMouseButtonDown(0)) animator.SetTrigger("Attack");
        if (Input.GetButtonDown("Jump")) animator.SetTrigger("Jump");
        if (Input.GetMouseButtonDown(1)) animator.SetTrigger("Dodge");
        if (Input.GetKeyDown(KeyCode.B)) animator.SetTrigger("Combat_Unarmed_Attack");
        // --- 2. ACTIONS (TRIGGERS) ---
    if (Input.GetMouseButtonDown(0)) 
    {
        animator.SetTrigger("Attack");
        
        // OPTIONAL: Snap to face the Boss when you click
        GameObject boss = GameObject.FindGameObjectWithTag("Enemy");
        if (boss != null)
        {
            Vector3 dirToBoss = (boss.transform.position - transform.position).normalized;
            dirToBoss.y = 0;
            if (dirToBoss != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(dirToBoss); // Instant snap
            }
        }}}
    public void DealDamage()
{
    // 1. Define the attack area
    Vector3 hitPosition = transform.position + transform.forward;
    float hitRadius = 2.0f; 
    Collider[] hitColliders = Physics.OverlapSphere(hitPosition, hitRadius);

    foreach (var hitCollider in hitColliders)
    {
        // 2. CHECK FOR ENEMY
        if (hitCollider.CompareTag("Enemy")) 
        {
            // 3. Create and position the hit effect
            if (hitEffectPrefab != null)
            {
                // We calculate the center of the enemy we actually HIT
                Vector3 centerOffset = new Vector3(0, 2, 0); 
                Vector3 spawnPos = hitCollider.transform.position + centerOffset;

                // We store the newly created object in a variable called 'effect'
                GameObject effect = Instantiate(hitEffectPrefab, spawnPos, Quaternion.identity);
                
                // Force scale to be normal regardless of Boss size
                effect.transform.localScale = Vector3.one;
                
                Destroy(effect, 3.0f); 
            }

            // 4. Deal the damage
            EnemyHealth health = hitCollider.GetComponentInParent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(25); 
                //return; // Hit one enemy and stop
            }
        }
    
        // 3. CHECK FOR MUSHROOM
        if (hitCollider.CompareTag("Mushroom"))
        {
            MushroomAI mushroom = hitCollider.GetComponent<MushroomAI>();
            if (mushroom != null)
            {
                Health playerHealth = GetComponent<Health>();
                if(playerHealth != null) 
                {
                    playerHealth.Heal(mushroom.healthAmount);
                }
                
                Destroy(hitCollider.gameObject);
                return;
            }
        }
    }
}
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, 1.5f);
    }
}