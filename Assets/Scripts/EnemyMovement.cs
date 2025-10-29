using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    [Header("Configuracao de Movimento")]
    public float velocidade = 2f;

    [Header("Deteccao de Chao/Borda")]
    public Transform sensorBorda;
    public float distanciaSensor = 0.5f;
    public LayerMask groundlayer;

    private bool walking = false;
    private float posSensorBordaInicialX;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (sensorBorda == null)
        {
            Debug.LogError("ERRO da borda");
        }
        else
        {
            posSensorBordaInicialX = Mathf.Abs(sensorBorda.localPosition.x);
        }

    }

    void Update()
    {
    
    }

    public void SetWalking(bool isWalking)
    {
        if (walking == isWalking) return;
        
        walking = isWalking;
        animator.SetBool("isWalking", walking);
    }

    public void TriggerAttack()
    {
        animator.SetTrigger("attack");
    }

    public void TriggerDeath()
    {
        animator.SetBool("isDead", true);
    }

    public void Movement(float direcaoX)
    {
        if (direcaoX > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direcaoX < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        bool hasGround = Physics2D.Raycast(sensorBorda.position, Vector2.down, distanciaSensor, groundlayer);

        if (!hasGround)
        {
            StopMovement();
            return; 
        }

        rb.linearVelocity = new Vector2(direcaoX * velocidade, rb.linearVelocity.y);

        SetWalking(true);
    }

    public void StopMovement()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        SetWalking(false);
    }
}
