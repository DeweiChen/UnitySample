using Unity.VisualScripting;
using UnityEngine;

public class ObjectManipulator : MonoBehaviour
{
    public Camera focusCamera;
    public GameObject targetObject;
    private Collider targetObjectCollider;
    
    public float zoomSpeed = 2f;
    public float minZoom = 5f;
    public float maxZoom = 100f;
    
    public float rotationSpeed = 90f;
    public float cameraRotationSpeed = 5f;
    
    void Update()
    {
        // Check for mouse click
        HandleSelectedObject();
        // handle Zoom in / out function
        HandleZoomInOut();
        // handle Rotation function
        HandleRotation();
    }

    private void HandleSelectedObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse position
            var ray = focusCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var raycastHit, 100f))
            {
                if (raycastHit.transform != null)
                {
                    targetObject = raycastHit.transform.gameObject;
                    targetObjectCollider = targetObject.GetComponent<Collider>();
                }
            }
        }
    }

    private void HandleRotation()
    {
        if (targetObject == null) return;
        
        // Check if the "R" key is pressed
        if (Input.GetKey(KeyCode.R))
        {
            // Rotate the GameObject when the "R" key is held down
            targetObject.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }

    private void HandleZoomInOut()
    {
        if (targetObject == null) return;

        // Zoom in and out using the mouse scroll wheel
        var scroll = Input.GetAxis("Mouse ScrollWheel");

        // Adjust the camera's position based on the scroll input
        var newZoom = Mathf.Clamp(focusCamera.fieldOfView - scroll * zoomSpeed, minZoom, maxZoom);
        focusCamera.fieldOfView = newZoom;
        var focusCameraTransform = focusCamera.GameObject().transform;
        
        // Calculate the rotation needed to look at the target
        Quaternion targetRotation = Quaternion.LookRotation(targetObjectCollider.bounds.center - focusCameraTransform.position, Vector3.up);
        // Smoothly interpolate between the current rotation and the target rotation
        focusCameraTransform.rotation = Quaternion.Slerp(focusCameraTransform.rotation, targetRotation, cameraRotationSpeed * Time.deltaTime);
    }
}
