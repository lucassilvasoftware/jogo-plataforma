using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int score = 0;
    public static int enemiesRemain;

    void Start()
    {
        enemiesRemain = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log("Inimigos restantes: " + enemiesRemain);
    }

    public static void AddScore(string enemyID)
    {
        if (enemyID != "Enemy") return;
        
        score += 100;
        enemiesRemain--;

        Debug.Log("Inimigos restantes: " + enemiesRemain);

        if (enemiesRemain <= 0)
        {
            Debug.Log("Todos os inimigos derrotados!");
            LoadNextLevel();
        }
    }

    private static void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene("WinScene");
        }
    }

    void Update()
    {
        
    }
}
