using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextView : MonoBehaviour
{

    private TextMeshProUGUI textMesh;
    private string text;

    public string Text { get { return text; } set { text = value; UpdateTextView(); } }

    private void Awake()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void UpdateTextView()
    {
        textMesh.text = text;
    }
}
