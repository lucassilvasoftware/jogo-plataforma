using UnityEngine;
using System;

public class HealthItem : MonoBehaviour
{
    public int healAmount = 1;
    public static event Action<int> OnHealthCollect;

    public void Collect()
    {
        OnHealthCollect.Invoke(healAmount);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o objeto que tocou o item tem a tag "Player"
        if (collision.CompareTag("Player"))
        {
            // Se for o player, chama a sua função de coleta
            Collect(); 
        }
    }
  
}
