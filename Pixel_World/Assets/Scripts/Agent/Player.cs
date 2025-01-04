using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Space;  // Make sure you have a 'World' script in the 'Space' namespace

namespace Agent
{
    public class Player : MonoBehaviour
    {
        public bool isGrounded;
        public bool isSprinting;

        private Transform cam;
        public World world;

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
        private float verticalMomentum = 0f;
        private bool jumpRequest;
        private float cameraPitch = 0f;
        private PlayerInputHandler inputHandler;

        // Three references we'll create dynamically at runtime
        public Transform targetBlock;     // The block to be removed
        public Transform placeBlock;      // The block to be placed
        public Text selectedBlockText;    // Text UI to display selected block info

        public byte selectedBlockIndex = 1;

        private void Start()
        {
            // 1) Create targetBlock at runtime if not assigned
            if (targetBlock == null)
            {
                GameObject targetObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                targetObj.name = "TargetBlock";
                targetObj.SetActive(false); // Hide initially
                targetBlock = targetObj.transform;
            }

            // 2) Create placeBlock at runtime if not assigned
            if (placeBlock == null)
            {
                GameObject placeObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                placeObj.name = "PlaceBlock";
                placeObj.SetActive(false); // Hide initially
                placeBlock = placeObj.transform;
            }

            // 3) Create a Canvas + Text for selectedBlockText if not assigned
            if (selectedBlockText == null)
            {
                // Create a Canvas
                GameObject canvasObj = new GameObject("RuntimeCanvas");
                Canvas canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();

                // Create the Text child
                GameObject textObj = new GameObject("SelectedBlockText");
                textObj.transform.SetParent(canvasObj.transform, false);

                // Add a Text component
                Text uiText = textObj.AddComponent<Text>();

                // Load font "MinecraftCHMC.ttf" from "Assets/Resources/Fonts/MinecraftCHMC.ttf"
                uiText.font = Resources.Load<Font>("Fonts/MinecraftCHMC");
                uiText.fontSize = 20;
                uiText.color = Color.white;
                uiText.text = "Selected Block: ???";

                RectTransform rectTransform = uiText.GetComponent<RectTransform>();
                rectTransform.anchorMin = new Vector2(0, 1); // top-left
                rectTransform.anchorMax = new Vector2(0, 1); // top-left
                rectTransform.anchoredPosition = new Vector2(150, -50);

                selectedBlockText = uiText;
            }

            // Find camera / world references if not already assigned
            cam = GameObject.Find("Main Camera")?.transform;
            if (!world)
                world = GameObject.Find("World")?.GetComponent<World>();

            // Disable script if critical references are missing
            if (cam == null || world == null)
            {
                Debug.LogError("Required components are missing. Ensure Main Camera and World are properly assigned.");
                enabled = false;
                return;
            }

            // Initialize selectedBlockText if valid
            if (selectedBlockText != null
                && world.blocktypes != null
                && world.blocktypes.Count > selectedBlockIndex)
            {
                selectedBlockText.text =
                    world.blocktypes[selectedBlockIndex].blockName + " block selected";
            }

            // Lock cursor (optional)
            Cursor.lockState = CursorLockMode.Locked;

            // Add PlayerInputHandler if you use a custom input system
            inputHandler = gameObject.AddComponent<PlayerInputHandler>();

            // Example spawn logic
            float centerX = VoxelData.WorldSizeInChunks * VoxelData.ChunkWidth / 2f;
            float centerZ = VoxelData.WorldSizeInChunks * VoxelData.ChunkWidth / 2f;
            float spawnY = VoxelData.ChunkHeight - 50f;
            transform.position = new Vector3(centerX, spawnY, centerZ);

            // Position the camera
            cam.position = transform.position + new Vector3(0, 1.6f, 0);
            cam.rotation = Quaternion.identity;

            Debug.Log("Player started. TargetBlock, PlaceBlock, and SelectedBlockText created at runtime.");
        }

        private void FixedUpdate()
        {
            HandleMovement();
            HandleCameraRotation();
            UpdateCameraPosition();
        }

        private void Update()
        {
            GetPlayerInputs();
        }

        private void HandleMovement()
        {
            CalculateVelocity();

            if (jumpRequest)
                Jump();

            transform.Translate(velocity, UnityEngine.Space.World);
        }

        private void CalculateVelocity()
        {
            verticalMomentum += gravity * Time.fixedDeltaTime;
            float speed = isSprinting ? sprintSpeed : walkSpeed;

            velocity = (transform.forward * vertical + transform.right * horizontal)
                       * speed * Time.fixedDeltaTime;
            velocity += Vector3.up * verticalMomentum * Time.fixedDeltaTime;

            if ((velocity.z > 0 && front) || (velocity.z < 0 && back))
                velocity.z = 0;
            if ((velocity.x > 0 && right) || (velocity.x < 0 && left))
                velocity.x = 0;

            if (velocity.y < 0)
                velocity.y = CheckDownSpeed(velocity.y);
            else
                velocity.y = CheckUpSpeed(velocity.y);
        }

        private void HandleCameraRotation()
        {
            transform.Rotate(Vector3.up * mouseHorizontal);

            cameraPitch -= mouseVertical;
            cameraPitch = Mathf.Clamp(cameraPitch, minVerticalAngle, maxVerticalAngle);
            cam.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
        }

        private void UpdateCameraPosition()
        {
            cam.position = transform.position + new Vector3(0, 1.6f, 0);
        }

        private void Jump()
        {
            verticalMomentum = jumpForce;
            isGrounded = false;
            jumpRequest = false;
        }

        private void GetPlayerInputs()
        {
            horizontal = inputHandler.movementInput.x;
            vertical = inputHandler.movementInput.y;
            mouseHorizontal = inputHandler.cameraInput.x;
            mouseVertical = inputHandler.cameraInput.y;
            isSprinting = inputHandler.isSprinting;

            if (isGrounded && inputHandler.jumpRequest)
                jumpRequest = true;

            // Destroy block (left-click)
            if (Input.GetMouseButtonDown(0))
            {
                if (targetBlock.gameObject.activeSelf)
                {
                    Vector3 pos = targetBlock.position;
                    world.SetVoxel(Mathf.FloorToInt(pos.x),
                                   Mathf.FloorToInt(pos.y),
                                   Mathf.FloorToInt(pos.z), 0);
                }
            }

            // Place block (right-click)
            if (Input.GetMouseButtonDown(1))
            {
                if (placeBlock.gameObject.activeSelf)
                {
                    Vector3 pos = placeBlock.position;
                    world.SetVoxel(Mathf.FloorToInt(pos.x),
                                   Mathf.FloorToInt(pos.y),
                                   Mathf.FloorToInt(pos.z),
                                   selectedBlockIndex);
                }
            }

            // Mouse wheel to change block type
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll > 0f)
            {
                selectedBlockIndex++;
                if (selectedBlockIndex >= world.blocktypes.Count)
                    selectedBlockIndex = 1;
                selectedBlockText.text = world.blocktypes[selectedBlockIndex].blockName + " block selected";
            }
            else if (scroll < 0f)
            {
                selectedBlockIndex--;
                if (selectedBlockIndex < 1)
                    selectedBlockIndex = (byte)(world.blocktypes.Count - 1);
                selectedBlockText.text = world.blocktypes[selectedBlockIndex].blockName + " block selected";
            }

            inputHandler.ResetInputs();
        }

        private float CheckDownSpeed(float downSpeed)
        {
            if (VoxelCheck(transform.position.x,
                           transform.position.y + downSpeed,
                           transform.position.z))
            {
                isGrounded = true;
                return 0f;
            }

            isGrounded = false;
            return downSpeed;
        }

        private float CheckUpSpeed(float upSpeed)
        {
            if (VoxelCheck(transform.position.x,
                           transform.position.y + 2f + upSpeed,
                           transform.position.z))
                return 0f;

            return upSpeed;
        }

        private bool VoxelCheck(float x, float y, float z)
        {
            return world.CheckForVoxel(x - playerWidth, y, z - playerWidth)
                   || world.CheckForVoxel(x + playerWidth, y, z - playerWidth)
                   || world.CheckForVoxel(x + playerWidth, y, z + playerWidth)
                   || world.CheckForVoxel(x - playerWidth, y, z + playerWidth);
        }

        public bool front
            => VoxelCheck(transform.position.x, transform.position.y, transform.position.z + playerWidth);
        public bool back
            => VoxelCheck(transform.position.x, transform.position.y, transform.position.z - playerWidth);
        public bool left
            => VoxelCheck(transform.position.x - playerWidth, transform.position.y, transform.position.z);
        public bool right
            => VoxelCheck(transform.position.x + playerWidth, transform.position.y, transform.position.z);
    }
}
