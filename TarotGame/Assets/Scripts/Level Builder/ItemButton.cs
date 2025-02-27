using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent<ItemButton> onClicked;

    private GameObject item = null;
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
    }

    public void SetItem(GameObject item) => this.item ??= item;
    public GameObject GetItem() => item;

    public void SetText(string text) => textDisplay.text = text;

    public void SetColor(Color color) => image.color = color;
    public void ResetColor() => image.color = defaultColor;
}
