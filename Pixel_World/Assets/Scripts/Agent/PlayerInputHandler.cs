using UnityEngine;
using UnityEngine.InputSystem;

namespace Agent{
    public class PlayerInputHandler : MonoBehaviour{
        public Vector2 movementInput; // Stores horizontal (A/D) and vertical (W/S) input
        public Vector2 cameraInput; // Stores mouse movement input
        public bool isSprinting;
        public bool isSneaking;
        public bool jumpRequest;
        public bool attackRequest;
        public bool useRequest;

        private void Update(){
            // Handle player movement (WASD)
            movementInput = new Vector2(
                (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0),
                (Keyboard.current.wKey.isPressed ? 1 : 0) - (Keyboard.current.sKey.isPressed ? 1 : 0)
            );

            // Handle camera movement (Mouse)
            cameraInput.x = Mouse.current.delta.x.ReadValue();
            cameraInput.y = Mouse.current.delta.y.ReadValue();

            // Sprinting (Left Shift)
            isSprinting = Keyboard.current.leftShiftKey.isPressed;

            // Sneaking (Left Control)
            isSneaking = Keyboard.current.leftCtrlKey.isPressed;

            // Jumping (Space)
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                jumpRequest = true;

            // Attacking/Breaking blocks (Left Mouse Button)
            if (Mouse.current.leftButton.wasPressedThisFrame)
                attackRequest = true;

            // Using/Placing blocks (Right Mouse Button)
            if (Mouse.current.rightButton.wasPressedThisFrame)
                useRequest = true;
        }

        public void ResetInputs(){
            // Reset one-time inputs like jump, attack, and use
            jumpRequest = false;
            attackRequest = false;
            useRequest = false;
        }
    }
}
