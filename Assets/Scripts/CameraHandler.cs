using UnityEngine;
using System.Collections;

public class CameraHandler : MonoBehaviour {

    private static readonly float PanSpeed = 20f;
    
    private static readonly float[] BoundsX = new float[]{-10f, 10f};
    private static readonly float[] BoundsY = new float[] { -10f, 10f };
    private static readonly float[] BoundsZ = new float[]{-18f, -4f};
    
    private Camera cam;
    
    private Vector3 lastPanPosition;
    private int panFingerId; // Touch mode only
    
    void Awake() {
        cam = GetComponent<Camera>();
    }
    
    void Update() {
        if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer) {
            HandleTouch();
        } else {
            HandleMouse();
        }
    }
    
    void HandleTouch() {
        switch (Input.touchCount)
        {

            case 1: // Panning
                    // If the touch began, capture its position and its finger ID.
                    // Otherwise, if the finger ID of the touch doesn't match, skip it.
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    lastPanPosition = touch.position;
                    panFingerId = touch.fingerId;
                }
                else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved)
                {
                    PanCamera(touch.position);
                }
                break;
        }
    }
    
    void HandleMouse() {
        // On mouse down, capture it's position.
        // Otherwise, if the mouse is still down, pan the camera.
        if (Input.GetMouseButtonDown(0)) {
            lastPanPosition = Input.mousePosition;
        } else if (Input.GetMouseButton(0)) {
            PanCamera(Input.mousePosition);
        }
    }
    
    void PanCamera(Vector3 newPanPosition) {
        // Determine how much to move the camera
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x * PanSpeed, offset.y * PanSpeed, 0);
        
        // Perform the movement
        transform.Translate(move, Space.World);  
        
        // Ensure the camera remains within bounds.
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, BoundsX[0], BoundsX[1]);
        pos.y = Mathf.Clamp(transform.position.y, BoundsY[0], BoundsY[1]);
        pos.z = Mathf.Clamp(transform.position.z, BoundsZ[0], BoundsZ[1]);
        transform.position = pos;
    
        // Cache the position
        lastPanPosition = newPanPosition;
    }
}