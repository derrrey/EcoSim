using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Camera Options
    [Header("Camera Options")]
    [SerializeField]
    float movementSpeed = 0.0f;
    [SerializeField]
    float boostSpeed = 0.0f;
    [SerializeField]
    float rotationSpeed = 0.0f;

    // State
    Transform cachedTransform = null;
    float currentSpeed = 0.0f;

    // Called once at startup
    void Start()
    {
        cachedTransform = GetComponent<Transform>();
        currentSpeed = movementSpeed;
    }

    // Called every frame
    void Update()
    {
        ProcessKeyboardInput();
        ProcessMouseInput();
    }

    // Processes the keyboard input and moves the camera accordingly
    void ProcessKeyboardInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            MoveInDirection(cachedTransform.forward);
        }
        if (Input.GetKey(KeyCode.A))
        {
            MoveInDirection(-cachedTransform.right);
        }
        if (Input.GetKey(KeyCode.S))
        {
            MoveInDirection(-cachedTransform.forward);
        }
        if (Input.GetKey(KeyCode.D))
        {
            MoveInDirection(cachedTransform.right);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            MoveInDirection(Vector3.up);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = boostSpeed;
        }
        else
        {
            currentSpeed = movementSpeed;
        }
    }

    // Moves the mouse in a specific direction
    void MoveInDirection(Vector3 direction)
    {
        cachedTransform.position += direction * currentSpeed * Time.deltaTime;
    }

    // Processes the mouse input and rotates the camera accordingly
    void ProcessMouseInput()
    {
        float mouseH = rotationSpeed * Input.GetAxis("Mouse X");
        float mouseV = rotationSpeed * Input.GetAxis("Mouse Y");

        cachedTransform.Rotate(-mouseV, 0, 0, Space.Self);
        cachedTransform.Rotate(0, mouseH, 0, Space.World);
    }
}
