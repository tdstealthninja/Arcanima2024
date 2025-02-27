using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject shadowTutorial;
    private static bool hasShownShadow = false;
    [SerializeField]
    private GameObject mirrorTutorial;
    private static bool hasShownMirror = false;

    private void Update()
    {
        FindObjectOfType<PlayerMovement>().moveEnabled = false;
    }

    public void CloseTutorial()
    {
        shadowTutorial.SetActive(false);
        mirrorTutorial.SetActive(false);
        gameObject.SetActive(false);

        FindObjectOfType<PlayerMovement>().moveEnabled = true;
    }

    public void ShowShadowTutorial()
    {
        if (!hasShownShadow)
        {
            shadowTutorial.SetActive(true);
            gameObject.SetActive(true);
            hasShownShadow = true;

            FindObjectOfType<PlayerMovement>().moveEnabled = false;
        }
    }
    public void ShowMirrorTutorial()
    {
        if (!hasShownMirror)
        {
            mirrorTutorial.SetActive(true);
            gameObject.SetActive(true);
            hasShownMirror = true;

            FindObjectOfType<PlayerMovement>().moveEnabled = false;
        }
    }
}
