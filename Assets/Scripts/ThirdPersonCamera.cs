using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;          
    public float distance = 6f;       
    public float height = 3f;        
    public float rotationSpeed = 5f;  
    public float smoothSpeed = 10f;   

    private float currentYaw = 0f;

    void LateUpdate()
    {
        if (target == null) return;

        currentYaw += Input.GetAxis("Mouse X") * rotationSpeed;

       
        Quaternion rotation = Quaternion.Euler(0, currentYaw, 0);

        Vector3 offset = rotation * Vector3.back * distance + Vector3.up * height;
        Vector3 desiredPosition = target.position + offset;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}