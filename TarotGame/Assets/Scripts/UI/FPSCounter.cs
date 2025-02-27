using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    TextMeshProUGUI textMesh;

    [SerializeField, Tooltip("in seconds")]
    private float updateDelay = 1f;

    private float currentFPS = 0;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();

        StartCoroutine(UpdateText());
    }

    // Update is called once per frame
    void Update()
    {
        float rawFPS = (1f / Time.unscaledDeltaTime);
        float twoDecimalFPS = (int)(rawFPS * 100) * 0.01f; // gets to two decimal places

        currentFPS = twoDecimalFPS;
    }

    private IEnumerator UpdateText()
    {
        while (enabled)
        {
            textMesh.text = "FPS: " + currentFPS;

            yield return new WaitForSeconds(updateDelay);
        }
    }
}
