using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnStartClick()
    {
        SceneManager.LoadScene("Fase 1");
    }

    public void OnMenuClick()
    {
        SceneManager.LoadScene("InitialScene");
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }
}
