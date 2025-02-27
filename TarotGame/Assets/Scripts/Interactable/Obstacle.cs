using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private Sprite enabledSprite;
    [SerializeField]
    private Sprite disabledSprite;

    private GridCollider collision;
    private GridCollider.CollisionFlags collisionFlags;
    private SpriteRenderer sr;

    [SerializeField]
    private bool startActive = true;
    private bool active = true;

    // Start is called before the first frame update
    void Start()
    {
        collision = GetComponent<GridCollider>();
        collisionFlags = collision.CollidesWithMe;
        sr = GetComponent<SpriteRenderer>();
        FindObjectOfType<StateChange>().onStateChange.RemoveListener(OnStateChange);
        FindObjectOfType<StateChange>().onStateChange.AddListener(OnStateChange);

        if (!startActive)
            OnStateChange();
    }

    public void OnStateChange()
    {
        /*
        active = !active;

        if (active)
        {
            sr.sprite = enabledSprite;
            collision.CollidesWithMe = collisionFlags;
        }
        else
        {
            sr.sprite = disabledSprite;
            collision.CollidesWithMe = 0;
        }
        */
    }
}
