using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class TouchInputManager : MonoBehaviour
{
    public static TouchInputManager Instance { get; private set; }

    private IsometricPlayerController playerControls;

    public delegate void OnStartTouch(Vector2 position, float time);
    public event OnStartTouch onStartTouch;
    public delegate void OnEndTouch(Vector2 position, float time);
    public event OnEndTouch onEndTouch;

    private void Awake()
    {
        Init();

        playerControls = new IsometricPlayerController();
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
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerControls.Touch.PrimaryContact.started -= StartTouchPrimary;
        playerControls.Touch.PrimaryContact.started += StartTouchPrimary;
        playerControls.Touch.PrimaryContact.canceled -= EndTouchPrimary;
        playerControls.Touch.PrimaryContact.canceled += EndTouchPrimary;
    }

    private void StartTouchPrimary(InputAction.CallbackContext context)
    {
        onStartTouch?.Invoke(playerControls.Touch.PrimaryPosition.ReadValue<Vector2>(), (float)context.startTime);
    }

    private void EndTouchPrimary(InputAction.CallbackContext context)
    {
        onEndTouch?.Invoke(playerControls.Touch.PrimaryPosition.ReadValue<Vector2>(), (float)context.startTime);
    }
}
