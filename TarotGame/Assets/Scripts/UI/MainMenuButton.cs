using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public void ToMainMenu()
    {
        SceneManager.LoadScene("TitleScene");
        Time.timeScale = 1;
    }
}
