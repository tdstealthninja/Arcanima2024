using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenScript : MonoBehaviour
{
    [SerializeField]
    private string main;

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep; // phone shouldn't sleep when the game is playing
    }

    public void OnGameStart() => SceneManager.LoadScene(main);

    public void OnGameQuit() => Application.Quit();
}
