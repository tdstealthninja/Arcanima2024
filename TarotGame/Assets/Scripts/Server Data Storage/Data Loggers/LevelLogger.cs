using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)] // start before the level loading
public class LevelLogger : MonoBehaviour
{
    private FPSLogger fpsLogger;
    private TimeLogger timeLogger;
    private ActionLogger actionLogger;

    // Targets
    [SerializeField]
    private PlayLoadedLevelV2 load;
    [SerializeField]
    private ServerAccess serverAccess;
    private LevelManagerV2 levelManager = LevelManagerV2.Instance;

    private IEncoder encoder = new JsonEncoder();

    delegate void Del();

    private void Awake()
    {
        fpsLogger = GetComponent<FPSLogger>();
        timeLogger = GetComponent<TimeLogger>();
        actionLogger = GetComponent<ActionLogger>();

        // privacy policy not accepted
        if(!PlayerID.privacyPolicyAccepted)
        {
            Destroy(fpsLogger.gameObject);
            Destroy(timeLogger.gameObject);
            Destroy(actionLogger.gameObject);
            Destroy(gameObject);
            return;
        }

        // Level loading delegates
        load.onLoadStarted.RemoveListener(timeLogger.Log);
        load.onLoadStarted.AddListener(timeLogger.Log);
        load.onLoadEnded.RemoveListener(timeLogger.Log);
        load.onLoadEnded.AddListener(timeLogger.Log);
        load.onLoadEnded.RemoveListener(OnLevelLoaded);
        load.onLoadEnded.AddListener(OnLevelLoaded);
        load.onLevelFinished.RemoveListener(OnLevelCompleted);
        load.onLevelFinished.AddListener(OnLevelCompleted);
    }

    private void OnLevelLoaded()
    {
        ActionData.SetOffsetToNow(); // reset the timer

        // set up interactable logs
        foreach (var interactable in FindObjectsOfType<Interactable>())
        {
            interactable.onInteractionSuccess.RemoveListener(actionLogger.LogInteraction);
            interactable.onInteractionSuccess.AddListener(actionLogger.LogInteraction);
        }

        // set up grid transform logs
        foreach (var gridTransform in FindObjectsOfType<GridTransform>())
        {
            gridTransform.onMoveSuccess.RemoveListener(actionLogger.LogGridMove);
            gridTransform.onMoveSuccess.AddListener(actionLogger.LogGridMove);
        }
    }

    private void OnLevelCompleted()
    {
        LevelDebugData data = new();

        data.id = PlayerID.id; // placeholder
        data.level = levelManager.GetCurrentLevel();
        data.loadTime = timeLogger.Return() as TimeData;
        data.framerate = fpsLogger.Return() as GraphData;
        data.actions = actionLogger.Return();

        string encoded = encoder.Encode(data);

        serverAccess.Store(encoded);
    }
}
