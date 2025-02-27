using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GraphViewElement : MonoBehaviour
{
    private float baseHeight;
    [SerializeField]
    private RectTransform childTransform;
    [SerializeField]
    private TextMeshProUGUI textMesh;

    private void Awake()
    {
        baseHeight = transform.parent.GetComponent<RectTransform>().rect.height;
    }

    public void SetValue(float min, float max, float value)
    {
        float t = Mathf.InverseLerp(min, max, value);
        Vector2 size = childTransform.sizeDelta;
        size.y = Mathf.Lerp(0, baseHeight, t);
        childTransform.sizeDelta = size;
        textMesh.text = value.ToString();
    }
}
