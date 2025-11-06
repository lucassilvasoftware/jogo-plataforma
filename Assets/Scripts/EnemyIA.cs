using UnityEngine;
using System.Collections.Generic;

public class EnemyIA : MonoBehaviour
{
    private EnemyMovement enemyMovement;
    public Transform player;
    public float speed = 2f;

    [Header("Distâncias do Inimigo ")]
    public float raioDeteccao = 5f;
    public float distanciaParaAtacar = 1.5f;

    public float attackCooldown = 1.15f;
    private float nextAttackTime = 0f;
    private bool isDead = false;
    private Rigidbody2D rb;
    public int damage = 1;

    //LootTable
    [Header("Loot")]
    public List<LootItem> lootTable = new List<LootItem>();
    
    void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else{
                Debug.LogError("Erro em InimigoIA: não acha um gameobject");
            }
        }
    }

    void Update()
    {
        if (player == null || enemyMovement == null || isDead) return;

        float distancia = Vector2.Distance(transform.position, player.position);

        if (distancia <= raioDeteccao)
        {
            if (distancia <= distanciaParaAtacar)
            {
                if (Time.time >= nextAttackTime)
                {
                    Debug.LogError("ordem attack agora");
                    enemyMovement.StopMovement();
                    enemyMovement.TriggerAttack();
                    nextAttackTime = Time.time + attackCooldown;

                    PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(damage);
                    }
                }
                else
                {
                    enemyMovement.StopMovement();
                }
            }
            else
            {
                nextAttackTime = Time.time;
                float direcaoX = Mathf.Sign(player.position.x - transform.position.x);
                enemyMovement.Movement(direcaoX);
            }
        } 
        else
        {
            enemyMovement.StopMovement();
        }
        
    }

    public void Death()
    {

        if (isDead) return;
        isDead = true;

        //Go around loottable
        //Spawn Item
        foreach (LootItem lootItem in lootTable)
        {
            if (Random.Range(0f, 100f) <= lootItem.dropChance)
            {
                InstantiateLoot(lootItem.itemPrefab);
            }
            break;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore("Enemy");
        }
        
        enemyMovement.TriggerDeath();

        this.enabled = false;
        enemyMovement.enabled = false;

        if (rb != null)
        {
            //rb.isKinematic = false;
            rb.bodyType = RigidbodyType2D.Dynamic;

            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        Destroy(gameObject, 2f);
    }

    void InstantiateLoot(GameObject loot)
    {
        if (loot)
        {
            GameObject droppedLoot = Instantiate(loot, transform.position, Quaternion.identity);

            droppedLoot.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    
}
