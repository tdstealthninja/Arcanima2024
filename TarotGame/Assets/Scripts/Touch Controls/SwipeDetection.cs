using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    public static SwipeDetection Instance;

    [SerializeField]
    private float minimumDistance = 10f;
    [SerializeField]
    private float maximumTime = 1f;

    public delegate void OnSwipe(Vector2 direction);
    public event OnSwipe onSwipe;

    private TouchInputManager inputManager;

    private Vector2 startPostion;
    private Vector2 endPosition;

    private float startTime;
    private float endTime;

    private void Awake()
    {
        Init();

        inputManager = TouchInputManager.Instance;
    }

    private void Init()
    {
        Destroy(Instance);

        // There can only be one instance of this object
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void OnEnable()
    {
        inputManager.onStartTouch += SwipeStart;
        inputManager.onEndTouch += SwipeEnd;
    }

    private void OnDisable()
    {
        inputManager.onStartTouch -= SwipeStart;
        inputManager.onEndTouch -= SwipeEnd;
    }

    private void SwipeStart(Vector2 position, float time)
    {
        startPostion = position;
        startTime = time;
    }

    private void SwipeEnd(Vector2 position, float time)
    {
        endPosition = position;
        endTime = time;

        DetectSwipe();
    }

    private void DetectSwipe()
    {
        Vector2 diff = endPosition - startPostion;

        if (diff.magnitude >= minimumDistance && (endTime - startTime) <= maximumTime)
            onSwipe?.Invoke(diff.normalized);
    }
}
