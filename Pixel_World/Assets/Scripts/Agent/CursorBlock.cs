using UnityEngine;
using Space;

namespace Agent{
    public class CursorBlock : MonoBehaviour{
        [Header("References")] 
        public Transform targetBlock; // Shows the block to be removed
        public Transform placeBlock; // Shows the block to be added
        public Transform playerCamera; // The player's or main camera's Transform

        [Header("Highlight Settings")] 
        public float checkIncrement = 0.1f; // Step length used for block detection
        public float reach = 8f; // Maximum detection range

        private World world;

        private void Start(){
            // If the playerCamera is not assigned in the Inspector, try to get it automatically
            if (playerCamera == null)
                playerCamera = Camera.main?.transform;

            // Find and assign the World script
            var worldObj = GameObject.Find("World");
            if (worldObj != null)
                world = worldObj.GetComponent<World>();
            else
                Debug.LogWarning("Cannot find a GameObject named 'World' to get the World script.");

            // Warn if targetBlock or placeBlock is not assigned in the Inspector
            if (targetBlock == null || placeBlock == null)
                Debug.LogWarning("Please assign targetBlock and placeBlock in the Inspector.");
        }

        private void Update(){
            if (playerCamera == null || world == null
                                     || targetBlock == null || placeBlock == null)
                return; // Return early if references are missing

            UpdateBlockPositions();
        }

        private void UpdateBlockPositions(){
            var step = 0f;
            var lastPos = Vector3.zero;

            // Disable both indicators before detection
            targetBlock.gameObject.SetActive(false);
            placeBlock.gameObject.SetActive(false);

            // Incrementally check positions along the player's forward direction
            while (step < reach){
                step += checkIncrement;
                var checkPos = playerCamera.position + playerCamera.forward * step;

                // Check if this position is a voxel (not air)
                if (world.CheckForVoxel(checkPos.x, checkPos.y, checkPos.z)){
                    // Move targetBlock to the detected voxel
                    targetBlock.position = new Vector3(
                        Mathf.FloorToInt(checkPos.x),
                        Mathf.FloorToInt(checkPos.y),
                        Mathf.FloorToInt(checkPos.z)
                    );

                    // Move placeBlock to the last known empty position
                    placeBlock.position = new Vector3(
                        Mathf.FloorToInt(lastPos.x),
                        Mathf.FloorToInt(lastPos.y),
                        Mathf.FloorToInt(lastPos.z)
                    );

                    // Enable both indicators
                    targetBlock.gameObject.SetActive(true);
                    placeBlock.gameObject.SetActive(true);

                    return; // Stop searching once a voxel is found
                }

                // Update lastPos if this position is empty
                lastPos = checkPos;
            }
        }
    }
}