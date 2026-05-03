using UnityEngine;
using UnityEngine.AI;

public class MushroomAI : MonoBehaviour
{
    private NavMeshAgent agent;
    public float roamRadius = 5f;
    public float waitTime = 3f;
    private float timer;

    private Animator anim;

    [Header("Health Settings")]
    public int healthAmount = 20;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = waitTime;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // If the mushroom reached its spot or waited long enough, find a new spot
        if (timer >= waitTime || !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            Vector3 newPos = RandomNavMeshLocation(roamRadius);
            agent.SetDestination(newPos);
            timer = 0;
        }
        if (anim != null && agent != null)
    {
        // This tells the Animator how fast the mushroom is physically moving
        float speed = agent.velocity.magnitude;
        anim.SetFloat("MoveSpeed", speed);
    }
    if (agent.velocity.magnitude > 0.1f)
    {
        // Make it tilt left and right as it walks
        float tilt = Mathf.Sin(Time.time * 10f) * 15f; 
        // Make it hop slightly
        float hop = Mathf.Abs(Mathf.Sin(Time.time * 10f)) * 0.2f;

        transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, tilt);
        transform.GetChild(0).localPosition = new Vector3(0, hop, 0);
    }
    }
    public Vector3 RandomNavMeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        
        // Finds the closest valid point on the blue NavMesh
        // Use NavMesh.SamplePosition to find a valid spot on the blue baked mesh
if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas)) 
{
    finalPosition = hit.position;
}
    
        return finalPosition;
    }

    // This is called when the Toad "destroys" or walks into the mushroom
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Assuming your Player has a script to handle health
            Health player = other.GetComponent<Health>();
            if (player != null)
            {
                player.Heal(healthAmount);
                Debug.Log("Mushroom consumed! Health restored.");
                Destroy(gameObject); // Remove mushroom from scene
            }
        }
    }
}