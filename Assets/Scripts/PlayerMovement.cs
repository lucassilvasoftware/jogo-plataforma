using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  [Header("Configurações de Movimento")]
  public float speed = 5f;
  public float jumpForce = 7f;
  public Transform groundCheck;      // Empty no pé do player
  public float groundCheckRadius = 0.2f;
  public LayerMask groundLayer;      // camada do chão

  private Rigidbody2D rb;
  private SpriteRenderer sr;
  private Animator anim;
  private bool isGrounded;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    sr = GetComponent<SpriteRenderer>();
    anim = GetComponent<Animator>();

    Debug.Log("✅ PlayerMovement ativo (Unity 6.x) — debug ON");
  }

  void Update()
  {
    // --- Movimento Horizontal ---
    float move = Input.GetAxis("Horizontal");
    rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

    // Direção visual
    if (move > 0) sr.flipX = false;
    else if (move < 0) sr.flipX = true;

    // Atualiza animação de movimento
    anim.SetFloat("Speed", Mathf.Abs(move));

    // --- Checa se está no chão ---
    isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

    // 🔹 DEBUG: mostra no console o estado
    Debug.Log($"isGrounded: {isGrounded}");

    // --- Pulo ---
    if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)) && isGrounded)
    {
      rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
      anim.SetBool("isJumping", true); // 👉 animação de pulo ON
      Debug.Log("🟡 PULO executado!");
    }

    // --- Atualiza animação de pulo ---
    if (isGrounded)
      anim.SetBool("isJumping", false); // 👉 volta pro Idle/Walk
  }

  // --- Gizmo visual pra ver o GroundCheck no editor ---
  void OnDrawGizmos()
  {
    if (groundCheck != null)
    {
      Gizmos.color = isGrounded ? Color.green : Color.red;
      Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
  }
}
