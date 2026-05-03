using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;          
    public float distance = 6f;       // Adjusted from 4
    public float height = 3f;         // Adjusted from 25 for better view
    public float rotationSpeed = 5f;  
    public float smoothSpeed = 10f;   

    private float currentYaw = 0f;

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Get Mouse Input
        currentYaw += Input.GetAxis("Mouse X") * rotationSpeed;

        // 2. Calculate the Direction
        // We create the rotation first so it's independent of the player's jitter
        Quaternion rotation = Quaternion.Euler(0, currentYaw, 0);

        // 3. Calculate Position
        // We calculate the offset (Back * Distance) + (Up * Height)
        Vector3 offset = rotation * Vector3.back * distance + Vector3.up * height;
        Vector3 desiredPosition = target.position + offset;

        // 4. Smoothly move (Increase smoothSpeed to 15 if it feels too 'laggy')
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // 5. Look at the Toad's head area
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}