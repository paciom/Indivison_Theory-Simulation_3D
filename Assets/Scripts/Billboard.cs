using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Rotate the triangles to face the camera's forward direction
        transform.forward = mainCamera.transform.forward;
    }
}
