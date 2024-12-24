using UnityEngine;
using AbstractClass.Subject;

namespace Agent {
    /// <summary>
    /// Represents the player entity, inheriting from Subject.
    /// </summary>
    public class Agent : Subject {
        private void Update() {
            if (IsDead) return;

            // Handle player input for movement
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(horizontal, 0, vertical);

            if (direction.magnitude > 0.01f) {
                Move(direction);
            }

            // Example: Attack on left mouse button (no actual target specified)
            if (Input.GetMouseButtonDown(0)) {
                Debug.Log("Player/Agent attempts to attack, but no target is specified.");
                // If there's a target, you can do: Attack(target, 10f);
            }
        }

        protected override void Die() {
            base.Die();
            // Custom player death logic
            Debug.Log("Player (Agent) died. Game Over logic can be placed here.");
        }
    }
}