using UnityEngine;

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

        enemyMovement.TriggerDeath();
        GameManager.AddScore("Enemy");

        this.enabled = false;
        GetComponent<Collider2D>().enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        rb.linearVelocity = Vector2.zero;

        Destroy(gameObject, 2f);
    }

    
}
