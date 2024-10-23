using UnityEngine;

public class AnimalHealth : MonoBehaviour{
    public int maxHealth = 20;
    private int currentHealth;
    private AnimalAI animalAI;

    void Start(){
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage){
        currentHealth -= damage;

        if (currentHealth <= 0){
            Die();
        }
    }

    void Die(){
        // Play death animation or effect if needed
        Destroy(gameObject);
    }
}