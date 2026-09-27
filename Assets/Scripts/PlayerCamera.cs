using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public InputActionReference mouseMovementInput;
    public float speed = 1;

    float xRotation = 0f;
    float yRotation = 0f;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        mouseMovementInput.action.Enable();
    }

    void Update()
    {
        Vector2 mouseDelta = mouseMovementInput.action.ReadValue<Vector2>();
        xRotation += mouseDelta.y * speed;
        yRotation += mouseDelta.x * speed;
        if (xRotation > 90) xRotation = 90;
        if (xRotation < -90) xRotation = -90;
        transform.rotation = Quaternion.Euler(-xRotation, yRotation, 0);
    }
    
}
