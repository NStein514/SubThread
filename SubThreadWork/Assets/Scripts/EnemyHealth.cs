using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3; 
    private int currentHealth;

    private Enemy enemy;

    void Awake()
    {
        currentHealth = maxHealth;
        enemy = GetComponent<Enemy>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Play the Death Effect
        Debug.Log($"{gameObject.name} defeated!");

        GameManager.Instance.AddScore(100);

        Destroy(gameObject);
    }
}
