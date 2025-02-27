using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AbilityButtonHandler : MonoBehaviour
{
    [SerializeField]
    private Button button;

    public UnityEvent onPress;

    private void Awake()
    {
        FindObjectOfType<PlayLoadedLevelV2>().onLevelFinished.RemoveListener(() => EnableButton(false));
        FindObjectOfType<PlayLoadedLevelV2>().onLevelFinished.AddListener(() => EnableButton(false));
    }

    public void EnableButton(bool active) => button.gameObject.SetActive(active);

    public void SetSprite(Sprite sprite) => button.image.sprite = sprite;

    public void OnAbilityButtonPressed() => onPress?.Invoke();
}
