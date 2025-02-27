using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueButton : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent<DialogueButton> onClicked;

    private Dialogue dialogue = null;
    private TextMeshProUGUI textDisplay;
    private Image image;

    private Color defaultColor;

    private void Awake()
    {
        textDisplay = GetComponentInChildren<TextMeshProUGUI>();
        image = GetComponent<Image>();

        defaultColor = image.color;
    }

    public void OnClick()
    {
        onClicked.Invoke(this);
        //Destroy(this.gameObject);
    }

    public void SetDialogue(Dialogue dialogue) => this.dialogue ??= dialogue;
    public Dialogue GetDialogue() => dialogue;
    public void SetText(string text) => textDisplay.text = text;
    public void SetColor(Color color) => image.color = color;
    public void ResetColor() => image.color = defaultColor;
}
