using UnityEngine;

public class PlayerController : MonoBehaviour{
    public float speed = 5.0f; // Movement speed
    public float lookSpeed = 2.0f; // Mouse sensitivity
    public float gravity = -9.81f; // Gravity value
    public float jumpHeight = 1.2f; // Jump height
    
    public Transform groundCheck; // Position to check if the player is grounded
    public float groundDistance = 0.8f; // Radius of the ground check sphere
    public LayerMask groundMask; // Layer mask to specify what is considered ground

    private CharacterController controller;
    private Transform cameraTransform;
    private Vector3 velocity; // Stores velocity for gravity and jumping
    private bool isGrounded; // Is the player grounded?

    private float xRotation = 0f;

    void Start(){
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked; // Lock cursor for first-person view
    }

    void Update(){
        GroundCheck();
        MovePlayer();
        LookAround();
        HandleJumpingAndGravity();
    }

    void MovePlayer(){
        // Player movement input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        controller.Move(move * speed * Time.deltaTime);
    }

    void LookAround(){
        // Get mouse input for looking around
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        // Rotate player body horizontally
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera vertically and clamp the rotation
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void GroundCheck(){
        // Check if the player is touching the ground using a sphere cast
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // If grounded and the player is falling (velocity.y < 0), reset the downward velocity
        if (isGrounded && velocity.y < 0){
            velocity.y = -2f; // Set to a small value to keep the player grounded
        }
        
        Debug.Log(groundCheck.position+" "+groundDistance+" "+groundMask);
        // Debugging: Print whether the player is grounded or not
        Debug.Log("Grounded: " + isGrounded);
    }

    void HandleJumpingAndGravity(){
        // Handle Jumping
        if (isGrounded && Input.GetButtonDown("Jump")){
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }


        controller.Move(velocity * Time.deltaTime);

        // Apply Gravity
        velocity.y += gravity * Time.deltaTime;
    }
}