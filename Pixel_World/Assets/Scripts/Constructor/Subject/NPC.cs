using UnityEngine;
using Constructor.Subject;

namespace NPC {
    /// <summary>
    /// Represents an NPC or enemy entity, inheriting from Subject.
    /// </summary>
    public class NPC : Subject {
        [Tooltip("NPC attack cooldown in seconds.")]
        public float attackCooldown = 3f;

        private float attackTimer;

        private void Update() {
            if (IsDead) return;

            // Simple AI example: patrol or wander around
            Patrol();

            // Attack logic with cooldown
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCooldown) {
                // Find a target to attack, for example the player
                Agent.Agent target = FindPlayer();
                if (target != null) {
                    Attack(target, 10f);
                }
                attackTimer = 0f;
            }
        }

        /// <summary>
        /// Simple patrol method, can be replaced with more advanced AI behavior.
        /// </summary>
        private void Patrol() {
            // Example: move forward continuously
            Move(transform.forward);
        }

        /// <summary>
        /// Example method to find the player (Agent).
        /// A more robust system would find all Agents or check distance, etc.
        /// </summary>
        /// <returns>The first Agent found in the scene, or null if none found.</returns>
        private Agent.Agent FindPlayer() {
            return GameObject.FindObjectOfType<Agent.Agent>();
        }

        protected override void Die() {
            base.Die();
            // Custom NPC death logic (e.g., drop loot, play animations)
            Debug.Log($"{name} died. Loot is dropped here.");
            // Example: Destroy(gameObject, 2f);
        }
    }
}