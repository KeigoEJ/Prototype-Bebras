using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject pauseMenuPanel; 

    private bool isPaused = false;

    void Update()
    {
        // Toggle pause on ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else          PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;                 // freeze all time-based updates
        pauseMenuPanel.SetActive(true);      // show pause UI
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);     // hide pause UI
        Time.timeScale = 1f;                 // un-freeze
        isPaused = false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
