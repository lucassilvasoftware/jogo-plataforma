using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;
    public HealthUI HealthUI;

    public float fallDeathY = -5f;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        HealthUI.SetMaxHeart(maxHealth);
        HealthItem.OnHealthCollect += Heal;
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyIA enemy = collision.GetComponent<EnemyIA>();

        if (enemy)
        {
            TakeDamage(enemy.damage);
        }
    }

    void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        HealthUI.UpdateHearts(currentHealth);
    }

    void Update()
    {
        if (isDead) return;

        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.LogWarning("Player takes 1 damage for testing.");
            TakeDamage(1);
        }

        if (transform.position.y < fallDeathY)
        {
            isDead = true;
            SceneManager.LoadScene("GameOver");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        HealthUI.UpdateHearts(currentHealth);

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }   
    }
}
