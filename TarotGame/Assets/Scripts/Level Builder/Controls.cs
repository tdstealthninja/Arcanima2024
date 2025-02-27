using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controls : MonoBehaviour
{
    public enum Status
    {
        Released, // Mouse is released
        Down, // Mouse was just pressed
        Pressed, // Mouse is pressed
        Up // Mouse was just released
    }

    [SerializeField]
    private Camera topDownCam;
    [SerializeField]
    private Transform cursor;
    private Vector3Int cursorGridPos;

    private RectTransform palette;

    private Placement placement;
    private Camera current;
    private TerrainGrid grid;
    private readonly Vector3 offset = new(0.5f, 0.5f, 0.5f);

    // Start is called before the first frame update
    void Start()
    {
        current = topDownCam;
        grid = FindObjectOfType<TerrainGrid>();
        palette = GameObject.Find("Palette").transform as RectTransform;
        placement = GetComponent<Placement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grid == null)
            grid = FindObjectOfType<TerrainGrid>();

        UpdateCursorPosition();

        // Check if over UI
        if (CheckMouseOverUI()) return;

        if (leftClickStatus == Status.Down)
        {
            placement.Place(cursorGridPos);
        }
        if (rightClickStatus == Status.Down)
        {
            placement.Remove(cursorGridPos);
        }
    }

    private void UpdateCursorPosition()
    {
        cursorGridPos = grid.GetClosestCell(GetMouseWorldPos() + offset);
        cursor.position = cursorGridPos;
    }

    /// <summary>
    /// Gets the position of the mouse in the world
    /// </summary>
    /// <returns></returns>
    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = mousePosition;
        mousePos.z = 10;
        mousePos = current.ScreenToWorldPoint(mousePos);
        return mousePos;
    }

    private bool CheckMouseOverUI()
    {
        Vector3 mousePos = mousePosition;

        // palette info
        float rectHeight = palette.rect.height;
        Vector2 position = palette.rect.center;

        float topEdge = position.y + rectHeight;

        return topEdge > mousePos.y;
    }

    #region Input Actions

    private Vector2 mousePosition = Vector2.zero;
    public void UpdateMousePosition(InputAction.CallbackContext context) => mousePosition = context.ReadValue<Vector2>();

    #region Left Click

    private bool leftClicked = false;
    private bool leftClickedLastFrame = false;
    private Status leftClickStatus = Status.Released;
    public void UpdateLeftStatus(InputAction.CallbackContext context)
    {
        leftClicked = context.performed;

        if (leftClicked && leftClickedLastFrame)
            leftClickStatus = Status.Pressed;
        else if (leftClicked && !leftClickedLastFrame)
            leftClickStatus = Status.Down;
        else if (!leftClicked && !leftClickedLastFrame)
            leftClickStatus = Status.Released;
        else if (!leftClicked && leftClickedLastFrame)
            leftClickStatus = Status.Up;

        leftClickedLastFrame = leftClicked;
    } 
    #endregion

    #region Right Click

    private bool rightClicked = false;
    private bool rightClickedLastFrame = false;
    private Status rightClickStatus = Status.Released;
    public void UpdateRightStatus(InputAction.CallbackContext context)
    {
        rightClicked = context.performed;

        if (rightClicked && rightClickedLastFrame)
            rightClickStatus = Status.Pressed;
        else if (rightClicked && !rightClickedLastFrame)
            rightClickStatus = Status.Down;
        else if (!rightClicked && !rightClickedLastFrame)
            rightClickStatus = Status.Released;
        else if (!rightClicked && rightClickedLastFrame)
            rightClickStatus = Status.Up;

        rightClickedLastFrame = rightClicked;
    }  
    #endregion

    #endregion
}
