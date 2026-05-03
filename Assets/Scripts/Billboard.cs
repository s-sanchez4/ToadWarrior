using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cam;

    void Start() => cam = Camera.main.transform;

    void LateUpdate()
    {
        // Makes the UI always look at the camera
        transform.LookAt(transform.position + cam.forward);
    }
}