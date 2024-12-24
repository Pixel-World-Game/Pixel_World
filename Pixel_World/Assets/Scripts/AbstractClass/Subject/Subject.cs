using UnityEngine;

namespace AbstractClass.Subject {
    /// <summary>
    /// Abstract base class that represents any interactive entity in the game, 
    /// such as player (Agent), NPC, or any other creature.
    /// </summary>
    public abstract class Subject : MonoBehaviour {
        [Header("---------- Subject Basic Properties ----------")]
        [Tooltip("Current health points.")]
        public float health = 100f;

        [Tooltip("Maximum health points.")]
        public float maxHealth = 100f;

        [Tooltip("Movement speed.")]
        public float moveSpeed = 3.0f;

        [Tooltip("Indicates if the entity is dead.")]
        public bool IsDead { get; private set; }

        /// <summary>
        /// Method for taking damage. Decreases health, and triggers death if health <= 0.
        /// </summary>
        /// <param name="damage">Amount of damage received.</param>
        public virtual void TakeDamage(float damage) {
            if (IsDead) return; // Do nothing if already dead

            health -= damage;
            if (health <= 0) {
                health = 0;
                Die();
            }
        }

        /// <summary>
        /// Basic movement method. Children can either override or call this method.
        /// </summary>
        /// <param name="direction">Movement direction.</param>
        public virtual void Move(Vector3 direction) {
            // Simple example using Transform.Translate.
            // In a real project, consider using CharacterController, NavMesh, or physics-based movement.
            transform.Translate(direction.normalized * moveSpeed * Time.deltaTime, Space.World);
        }

        /// <summary>
        /// Basic attack method, children can override if needed.
        /// </summary>
        /// <param name="target">The target to attack, must be a Subject.</param>
        /// <param name="damage">Amount of damage dealt to the target.</param>
        public virtual void Attack(Subject target, float damage) {
            Debug.Log($"{name} is attacking {target.name} and deals {damage} damage.");
            target.TakeDamage(damage);
        }

        /// <summary>
        /// Called when health <= 0. Sets IsDead = true and triggers death logic.
        /// Children can override this method for custom death logic.
        /// </summary>
        protected virtual void Die() {
            IsDead = true;
            Debug.Log($"{name} has died.");
            // Common death handling (e.g., play animations, hide entity, etc.)
            // Example: Destroy(gameObject, 2f); // Destroy the entity after 2 seconds
        }
    }
}
