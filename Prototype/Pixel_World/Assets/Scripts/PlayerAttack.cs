using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int attackDamage = 5;
    public float attackRange = 2f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button to attack
        {
            Attack();
        }
    }

    void Attack()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackRange))
        {
            AnimalHealth animal = hit.collider.GetComponent<AnimalHealth>();

            if (animal != null)
            {
                animal.TakeDamage(attackDamage);
            }
        }
    }
}