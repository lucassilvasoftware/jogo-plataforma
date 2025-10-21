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

  private Rigidbody2D rb;
  private SpriteRenderer sr;
  private Animator anim;
  private bool isGrounded;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    sr = GetComponent<SpriteRenderer>();
    anim = GetComponent<Animator>();
  }

  void Update()
  {
    // Movimento horizontal
    float move = Input.GetAxis("Horizontal");
    rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

    // Vira sprite
    if (move > 0) sr.flipX = false;
    else if (move < 0) sr.flipX = true;

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
      Debug.Log($"⬆️ PULO!  | Grounded: {isGrounded} | VelocityY: {rb.linearVelocity.y}");
    }

    // Debug contínuo (a cada frame)
    Debug.Log($"Frame {Time.frameCount} | Grounded: {isGrounded} | isJumping(anim): {anim.GetBool("isJumping")}");
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
