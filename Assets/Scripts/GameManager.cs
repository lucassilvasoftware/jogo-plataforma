using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int score = 0;
    public int enemiesRemain;
    private TextMeshProUGUI scoreText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "WinScreen" && scene.name != "GameOver")
        {
            enemiesRemain = GameObject.FindGameObjectsWithTag("Enemy").Length;
            Debug.Log("Inimigos restantes: " + enemiesRemain);

            GameObject scoreTextObject = GameObject.Find("ScoreText");
            if (scoreTextObject != null)
            {
                scoreText = scoreTextObject.GetComponent<TextMeshProUGUI>();
                UpdateScoreUI();
            }
            else
            {
                scoreText = null;
            }
        }
        else
        {
            scoreText = null;
        }
    }

    /*void Start()
    {
        enemiesRemain = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log("Inimigos restantes: " + enemiesRemain);
    }*/

    public void AddScore(string enemyID)
    {

        Debug.LogWarning("Adicionando pontuação por inimigo derrotado: " + enemyID);
        if (enemyID != "Enemy") return;
        
        score += 100;
        enemiesRemain--;
        UpdateScoreUI();
        Debug.Log("Inimigos restantes: " + enemiesRemain);

        if (enemiesRemain <= 0)
        {
            StartCoroutine(LoadNextLevel(2.0f));
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    private IEnumerator LoadNextLevel(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        if (nextSceneIndex < totalScenes)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene("WinScene");
        }
    }

}
