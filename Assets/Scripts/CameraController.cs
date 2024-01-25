
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 1f;
    public float panSpeed = 2f;
    public float zoomSpeed = 2f;

    private Vector3 lastMousePosition;
    private Vector3 panOrigin;
    private bool isPanning = false;

    private void Update()
    {
        // Zoom using mouse scroll wheel
        float zoomAmount = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        Vector3 zoom = transform.forward * zoomAmount;
        transform.position += zoom;

        // Panning with right mouse button
        if (Input.GetMouseButtonDown(1))
        {
            lastMousePosition = Input.mousePosition;
            isPanning = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isPanning = false;
        }

        if (isPanning)
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 pan = transform.right * -delta.x / Screen.width * panSpeed + transform.up * -delta.y / Screen.height * panSpeed;
            transform.position += pan;
        }

        lastMousePosition = Input.mousePosition;

        // Rotate around the target using left mouse button
        if (Input.GetMouseButton(0))
        {
            float horizontalInput = Input.GetAxis("Mouse X");
            float verticalInput = Input.GetAxis("Mouse Y");

            transform.RotateAround(target.position, Vector3.up, horizontalInput * rotationSpeed);
            transform.RotateAround(target.position, transform.right, -verticalInput * rotationSpeed);
        }
    }
}