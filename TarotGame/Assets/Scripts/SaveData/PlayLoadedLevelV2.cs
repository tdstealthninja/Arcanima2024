using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using OneRoom;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayLoadedLevelV2 : MonoBehaviour
{
    public static Level levelObj;

    [Header("Components")]
    [SerializeField]
    private SaveGTDataManagerV2 dataManager;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject shadowPrefab;
    [SerializeField]
    private LoadingScreen loadingScreen;

    public bool shouldEndRealm = false;

    public UnityEvent onLoadStarted; 
    public UnityEvent onLoadEnded;
    public UnityEvent onLoadEndedLate;
    public UnityEvent onLevelFinished;

    private void Awake()
    {
        if (levelObj == null)
        {
            Debug.LogError("This scene cannot be loaded directly. Please load using LoadLevel.Load(levelObj)");
            return;
        }

        LoadLevel(levelObj);
    }

    public void LevelCompleted()
    {
        // prevent the player from moving
        PlayerMovement pm = FindObjectOfType<PlayerMovement>();
        pm.moveEnabled = false;

        onLevelFinished?.Invoke();

        StartCoroutine(WaitToComplete());
    }

    private IEnumerator WaitToComplete()
    {
        yield return new WaitForSeconds(2.5f);

        Debug.LogWarning("EndingRealm");

        if (shouldEndRealm)
        {
            SceneManager.LoadScene("LevelSelectScene");
        }
        else
        {
            LevelManagerV2 lm = LevelManagerV2.Instance;
            int currentLevel = lm.GetCurrentLevel();

            // Set up loading screen
            loadingScreen.SetDialogueText(lm.GetLevelDialogue(currentLevel));
            loadingScreen.Activate();
            currentLevel++;
            lm.SetCurrentLevel(currentLevel);

            // load the level
            if (!lm.InvalidIndex())
                LoadLevel(lm.GetLevelObj(currentLevel));
        }
    }

    public void LoadLevel(Level level)
    {
        levelObj = level;
        StartCoroutine(WaitToLoad());
    }

    private IEnumerator WaitToLoad()
    {
        yield return new WaitForSeconds(1);

        ClearLevel();

        LoadLevel();
    }

    public void LoadLevel()
    {
        onLoadStarted?.Invoke();

        Debug.Log("Started Loading Level");
        // sets level to load and then loads it
        dataManager.SetCurrentLoadLevel(levelObj);

        StaticBatchingUtility.Combine(FindObjectOfType<TerrainGrid>().gameObject);

        Debug.Log("Finished Loading Level");

        try
        {
            // reset rain to fall
            Rain[] rains = FindObjectsOfType<Rain>();
            foreach (Rain rain in rains)
                rain.ScaleEffect(1);
        }
        catch { } // catch prevents an error from stopping the code below it running (i'm to lazy to fix it now)

        onLoadEnded?.Invoke();

        onLoadEndedLate?.Invoke();

        // updates sprites to face the camera
        BillboardManager.Instance.UpdateSprites(cam);

        // No longer loading
        loadingScreen.ToggleLoading();
    }

    public void ClearLevel()
    {
        TerrainGrid tg = FindObjectOfType<TerrainGrid>();

        tg?.ClearLevel();

        if (tg != null)
            Destroy(tg.gameObject);
    }
}
