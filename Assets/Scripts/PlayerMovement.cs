using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  [Header("Movement Settings")]
  public float speed = 5f;
  public float jumpForce = 7f;

  [Header("Ground Check")]
  public Transform groundCheck;         // Objeto vazio sob o player
  public float groundRadius = 0.2f;
  public LayerMask groundLayer;         // Layer "Ground"

  [Header("Attack Settings")]
  public Transform attackPoint;
  public float attackRange = 0.5f;
  public LayerMask enemyLayers;

  private Rigidbody2D rb;
  private SpriteRenderer sr;
  private Animator anim;
  private bool isGrounded;
  private float posAttackPointInicialX;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    sr = GetComponent<SpriteRenderer>();
    anim = GetComponent<Animator>();

    if (attackPoint == null)
    {
      Debug.LogError("Erro: attackPoint não atribuído.");
    }
    else
    {
      posAttackPointInicialX = Mathf.Abs(attackPoint.localPosition.x);
    }
  }

  void Update()
  {
    if(Input.GetMouseButtonDown(0))
    {
      anim.SetTrigger("attack");
      Debug.Log("Ataque acionado");
      Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
      Debug.Log($"Inimigos atingidos: {hitEnemies.Length}");
      foreach(Collider2D enemy in hitEnemies)
      {
        Debug.Log("Inimigo atingido: " + enemy.name);
        EnemyIA enemyIA = enemy.GetComponent<EnemyIA>();
        if(enemyIA != null)
        {
          Debug.Log("Chamando Death no inimigo: ");
          enemyIA.Death();
        }
        else{
          Debug.LogError("EnemyIA não encontrado no inimigo atingido.");
        }
      }
    }
    

    // Movimento horizontal
    float move = Input.GetAxis("Horizontal");
    rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

    // Vira sprite
    if (move > 0)
    {
      sr.flipX = false;
      attackPoint.localPosition = new Vector3(posAttackPointInicialX, attackPoint.localPosition.y, attackPoint.localPosition.z);
    }
    else if (move < 0)
    {
      sr.flipX = true;
      attackPoint.localPosition = new Vector3(-posAttackPointInicialX, attackPoint.localPosition.y, attackPoint.localPosition.z);
    }

    // Atualiza animação de movimento
    anim.SetFloat("Speed", Mathf.Abs(move));

    // Checa se está no chão
    bool wasGrounded = isGrounded;
    isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

    if (isGrounded != wasGrounded)
    {
      Debug.Log($"🟢 Grounded mudou: {wasGrounded} → {isGrounded} (Posição: {groundCheck.position})");
    }

    anim.SetBool("isJumping", !isGrounded);

    // Pulo
    if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)) && isGrounded)
    {
      rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
      //Debug.Log($"⬆️ PULO!  | Grounded: {isGrounded} | VelocityY: {rb.linearVelocity.y}");
    }

    // Debug contínuo (a cada frame)
    //Debug.Log($"Frame {Time.frameCount} | Grounded: {isGrounded} | isJumping(anim): {anim.GetBool("isJumping")}");
  }

  private void OnDrawGizmosSelected()
  {
    if (groundCheck != null)
    {
      Gizmos.color = isGrounded ? Color.green : Color.red;
      Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
  }
}
