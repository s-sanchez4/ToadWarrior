using UnityEngine;

public partial class PlayerMovement : MonoBehaviour
{
  
    private Animator animator;
    private float Speed;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Get movement input (WASD or Arrow Keys)
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Speed = Mathf.Abs(vertical) + Mathf.Abs(horizontal);
        Speed = Mathf.Clamp01(Speed);

        // Set Speed parameter for the blend tree
        animator.SetFloat("Speed", Speed);

        // Handle attack input
        if (Input.GetMouseButtonDown(0)) // Left click to attack
        {
            animator.SetTrigger("Attack");
        }
        if (Input.GetMouseButtonDown(1)) // Right click to block
        {
            animator.SetTrigger("Combat_Unarmed_Dodge");
        }
        if (Input.GetKeyDown(KeyCode.Space)) // Space to jump
        {
            animator.SetTrigger("Jump_Start");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            animator.SetTrigger("Combat_Unarmed_Attack");
        }

    }
}