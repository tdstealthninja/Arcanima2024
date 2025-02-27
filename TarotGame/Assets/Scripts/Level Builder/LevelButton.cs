using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent<LevelButton> onClicked;

    private string fileName;
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
        onClicked?.Invoke(this);
    }

    public void SetFileName(string fileName) => this.fileName ??= fileName;
    public string GetFileName() => fileName;

    public void SetText(string text) => textDisplay.text = text;

    public void SetColor(Color color) => image.color = color;
    public void ResetColor() => image.color = defaultColor;
}
