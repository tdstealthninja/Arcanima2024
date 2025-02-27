using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenuButtons : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseButton;
    public GameObject invertButton;
    public GameObject reloadButton;
    bool paused = false;



    public void TogglePauseMenu()
    {
        if (pauseMenu != null && pauseButton != null)
        {
            if (!paused)
            {
                pauseButton.SetActive(false);
                pauseMenu.SetActive(true);
                invertButton.SetActive(false);
                reloadButton.SetActive(false);
                paused = true;
                Time.timeScale = 0;
            }
            else
            {
                pauseButton.SetActive(true);
                pauseMenu.SetActive(false);
                invertButton.SetActive(true);
                reloadButton.SetActive(true);
                paused = false;
                Time.timeScale = 1;
            }
            
        }
        
    }


    public void ToLevelSelect()
    {
        SceneManager.LoadScene("LevelSelectScene");
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
        Application.Quit();
    }

}
