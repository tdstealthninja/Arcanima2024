using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelV2 : MonoBehaviour
{
    [SerializeField]
    private string sceneName;
    public void Load(int levelIndex)
    {
        LevelManager.Instance.SetCurrentLevel(levelIndex);
        SceneManager.LoadScene(sceneName);
    }
    public void LoadFromObject(int levelIndex)
    {
        PlayLoadedLevelV2.levelObj = LevelManagerV2.Instance.GetLevelObj(levelIndex);
        LevelManagerV2.Instance.SetCurrentLevel(levelIndex);
        SceneManager.LoadScene(sceneName);
    }
}
