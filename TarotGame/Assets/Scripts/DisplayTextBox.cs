using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayTextBox : MonoBehaviour
{
    [TextArea, SerializeField]
    private string text;

    private TextMeshProUGUI textbox;

    private void Awake()
    {
        textbox = GameObject.Find("Textbox").GetComponent<TextMeshProUGUI>();
    }

    public void Toggle()
    {
        if (textbox.enabled)
            Hide();
        else
            Display();
    }

    public void Display()
    {
        textbox.text = text;

        textbox.enabled = true;
    }

    public void Hide()
    {
        textbox.enabled = false;
    }
}
