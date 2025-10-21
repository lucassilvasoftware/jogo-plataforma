using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  public float speed = 5f;
  public float jumpForce = 7f;
  private Rigidbody2D rb;
  private bool isGrounded;
  private SpriteRenderer sr;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
    sr = GetComponent<SpriteRenderer>();
  }

  void Update()
  {
    float move = Input.GetAxis("Horizontal");

    // Movimento horizontal
    rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

    // Virar sprite conforme direção
    if (move > 0) sr.flipX = false;
    else if (move < 0) sr.flipX = true;

    // Pulo
    if (Input.GetButtonDown("Jump") && isGrounded)
    {
      rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
      isGrounded = false;
    }
  }

  // Detectar se está no chão
  void OnCollisionEnter2D(Collision2D col)
  {
    if (col.collider.CompareTag("Ground"))
    {
      isGrounded = true;
    }
  }
}
