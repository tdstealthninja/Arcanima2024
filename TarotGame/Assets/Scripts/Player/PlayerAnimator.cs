using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerAnimator : MonoBehaviour
{
    private static bool crown = false;
    private static bool necklace = false;

    private static UnityEvent updateVisuals = new();

    private Animator animator;

    public GameObject crownObj;
    public GameObject necklaceObj;

    // Start is called before the first frame update
    void Start()
    {
        animator = FindObjectOfType<Animator>();
        updateVisuals.AddListener(UpdateVisuals);
        updateVisuals.Invoke();
    }

    private void OnDestroy()
    {
        updateVisuals.RemoveListener(UpdateVisuals);
    }

    private void UpdateVisuals()
    {
        crownObj.SetActive(crown);
        necklaceObj.SetActive(necklace);
    }

    public void Move(Vector2 direction)
    {
        if (animator == null) return;

        direction = Quaternion.Euler(0, 0, -45) * -direction;

        Debug.Log(gameObject.name + " " + direction);

        // parse touch controls into one direction (cannot move diagonal)
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x < 0)
                animator.SetTrigger("MoveRight");
            else
                animator.SetTrigger("MoveLeft");
        }
        else if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            if (direction.y < 0)
                animator.SetTrigger("MoveUp");
            else
                animator.SetTrigger("MoveDown");
        }
        else
            return; // perfect diagonal swipe (rare)
    }

    public void Push(Vector2 direction)
    {
        if (animator == null) return;

        direction = Quaternion.Euler(0, 0, -45) * -direction;

        // parse touch controls into one direction (cannot move diagonal)
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
                animator.SetTrigger("PushLeft");
            else
                animator.SetTrigger("PushRight");
        }
        else if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            if (direction.y > 0)
                animator.SetTrigger("PushRight");
            else
                animator.SetTrigger("PushLeft");
        }
        else
            return; // perfect diagonal swipe (rare)
    }
    
    public static void SetCrown(bool b)
    {
        crown = b;
        updateVisuals.Invoke();
    }

    public static void SetNecklace(bool b)
    {
        necklace = b;
        updateVisuals.Invoke();
    }
}
