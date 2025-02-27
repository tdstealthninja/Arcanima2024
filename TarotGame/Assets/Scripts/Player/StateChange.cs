using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StateChange : MonoBehaviour
{
    public UnityEvent onStateChange;
    public UnityEvent onLateStateChange;

    [SerializeField]
    private int count = 4;
    [SerializeField]
    private Sprite[] sprites;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        FindObjectOfType<PlayLoadedLevelV2>().onLevelFinished.AddListener(() => count = 0);
        UpdateVisuals();
    }

    public void ChangeState()
    {
        if (count > 0)
        {
            count--;
            onStateChange?.Invoke();
            onLateStateChange?.Invoke();
            UpdateVisuals();
        }
    }

    public void AddCount()
    {
        count++;
        UpdateVisuals();
    }

    private void UpdateVisuals() => image.sprite = sprites[count];
}
