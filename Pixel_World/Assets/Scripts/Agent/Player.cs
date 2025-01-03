using UnityEngine;
using Space;

namespace Agent
{
    public class Player : MonoBehaviour
    {
        public bool isGrounded;
        public bool isSprinting;

        private Transform cam;
        private World world;

        public float walkSpeed = 3f;
        public float sprintSpeed = 6f;
        public float jumpForce = 5f;
        public float gravity = -9.8f;

        public float playerWidth = 0.15f;
        public float boundsTolerance = 0.1f;

        public float minVerticalAngle = -60f;
        public float maxVerticalAngle = 60f;

        private float horizontal;
        private float vertical;
        private float mouseHorizontal;
        private float mouseVertical;
        private Vector3 velocity;
        private float verticalMomentum = 0;
        private bool jumpRequest;
        private float cameraPitch = 0f;
        private PlayerInputHandler inputHandler;

        private void Start()
        {
            cam = GameObject.Find("Main Camera")?.transform;
            world = GameObject.Find("World")?.GetComponent<Space.World>();

            if (cam == null || world == null)
            {
                Debug.LogError("Required components are missing. Ensure Main Camera and World are properly assigned.");
                enabled = false;
                return;
            }

            inputHandler = gameObject.AddComponent<PlayerInputHandler>();

            // Suppose your world is (chunkCount * chunkSize) wide
            // and you want the center at half that in X and Z, 
            // and a safe Y like 64:
            float centerX = (VoxelData.WorldSizeInChunks * VoxelData.ChunkWidth) / 2f;
            float centerZ = (VoxelData.WorldSizeInChunks * VoxelData.ChunkWidth) / 2f;
            float spawnY = VoxelData.ChunkHeight - 50;

            transform.position = new Vector3(centerX, spawnY, centerZ);

            // Now set the camera above the player's transform
            cam.position = transform.position + new Vector3(0, 1.6f, 0);
            cam.rotation = Quaternion.identity;

            Debug.Log("Spawned player at: " + transform.position);
        }

        private void FixedUpdate()
        {
            HandleMovement();
            HandleCameraRotation();
            UpdateCameraPosition();
        }

        private void HandleMovement()
        {
            CalculateVelocity();

            if (jumpRequest)
                Jump();

            // Fully qualify Unity space to avoid conflicts with namespace "Space"
            transform.Translate(velocity, UnityEngine.Space.World);
        }

        private void HandleCameraRotation()
        {
            // Rotate the player left/right
            transform.Rotate(Vector3.up * mouseHorizontal);

            // Tilt the camera up/down
            cameraPitch -= mouseVertical;
            cameraPitch = Mathf.Clamp(cameraPitch, minVerticalAngle, maxVerticalAngle);
            cam.localRotation = Quaternion.Euler(cameraPitch, 0, 0);
        }

        private void UpdateCameraPosition()
        {
            cam.position = transform.position + new Vector3(0, 1.6f, 0);
        }

        private void Update()
        {
            GetPlayerInputs();
        }

        private void Jump()
        {
            verticalMomentum = jumpForce;
            isGrounded = false;
            jumpRequest = false;
        }

        private void CalculateVelocity()
        {
            // Apply gravity unconditionally (or clamp for terminal velocity if desired)
            verticalMomentum += gravity * Time.fixedDeltaTime;

            // Determine movement speed (walk vs sprint)
            float speed = isSprinting ? sprintSpeed : walkSpeed;

            // Calculate horizontal movement
            velocity = (transform.forward * vertical + transform.right * horizontal) * speed * Time.fixedDeltaTime;

            // Add vertical momentum
            velocity += Vector3.up * verticalMomentum * Time.fixedDeltaTime;

            // XZ collisions
            if ((velocity.z > 0 && front) || (velocity.z < 0 && back))
                velocity.z = 0;

            if ((velocity.x > 0 && right) || (velocity.x < 0 && left))
                velocity.x = 0;

            // Y collisions
            if (velocity.y < 0)
                velocity.y = CheckDownSpeed(velocity.y);
            else
                velocity.y = CheckUpSpeed(velocity.y);
        }

        private void GetPlayerInputs()
        {
            horizontal = inputHandler.movementInput.x;
            vertical = inputHandler.movementInput.y;
            mouseHorizontal = inputHandler.cameraInput.x;
            mouseVertical = inputHandler.cameraInput.y;
            isSprinting = inputHandler.isSprinting;

            if (isGrounded && inputHandler.jumpRequest)
            {
                jumpRequest = true;
            }

            inputHandler.ResetInputs();
        }

        private float CheckDownSpeed(float downSpeed)
        {
            // Check if the space below is solid
            if (VoxelCheck(transform.position.x, transform.position.y + downSpeed, transform.position.z))
            {
                isGrounded = true;
                return 0; // Stop falling
            }

            isGrounded = false;
            return downSpeed;
        }

        private float CheckUpSpeed(float upSpeed)
        {
            // Check if the space above the player's head is solid
            if (VoxelCheck(transform.position.x, transform.position.y + 2f + upSpeed, transform.position.z))
                return 0; // Stop upward movement

            return upSpeed;
        }

        private bool VoxelCheck(float x, float y, float z)
        {
            // Check four corners horizontally for collisions
            return world.CheckForVoxel(x - playerWidth, y, z - playerWidth) ||
                   world.CheckForVoxel(x + playerWidth, y, z - playerWidth) ||
                   world.CheckForVoxel(x + playerWidth, y, z + playerWidth) ||
                   world.CheckForVoxel(x - playerWidth, y, z + playerWidth);
        }

        // Helpers to check if there's a block in front/back/left/right
        public bool front => VoxelCheck(transform.position.x, transform.position.y, transform.position.z + playerWidth);
        public bool back  => VoxelCheck(transform.position.x, transform.position.y, transform.position.z - playerWidth);
        public bool left  => VoxelCheck(transform.position.x - playerWidth, transform.position.y, transform.position.z);
        public bool right => VoxelCheck(transform.position.x + playerWidth, transform.position.y, transform.position.z);
    }
}
