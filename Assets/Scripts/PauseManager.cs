using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public bool gameIsPaused;
    public GameObject pauseUI;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
    }
    void PauseGame()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 0f;
            pauseUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseUI.SetActive(false);
        }
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        gameIsPaused = false;
    }
    public void MainMenu()
    {
        Time.timeScale = 1;
    }
}
