using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField]
    private Image background;
    [SerializeField]
    private TextMeshProUGUI dialogue;
    [SerializeField]
    private GameObject loadingAnim;
    [SerializeField]
    private GameObject tapToContinue;

    private bool acceptTap = false;

    private void Awake()
    {
        TouchInputManager.Instance.onStartTouch -= Instance_onStartTouch;
        TouchInputManager.Instance.onStartTouch += Instance_onStartTouch;
    }

    private void Instance_onStartTouch(Vector2 position, float time)
    {
        OnTap();
    }

    public void SetDialogueText(string text) => dialogue.text = text;

    public void Activate()
    {
        gameObject.SetActive(true);
        tapToContinue.SetActive(false);
        StartCoroutine(BackgroundFade(0, 1, 0.5f));
    }

    public void Deactivate()
    {
        tapToContinue.SetActive(false);
        loadingAnim.SetActive(false);
        StartCoroutine(BackgroundFade(1, 0, 0.01f));
    }

    public void ToggleLoading()
    {
        acceptTap = true;
        loadingAnim.SetActive(false);
        tapToContinue.SetActive(true);
    }

    public void OnTap()
    {
        if (acceptTap)
        {
            if (LevelManagerV2.Instance.InvalidIndex())
            {
                SceneManager.LoadScene("EndingScene");
                return;
            }

            Deactivate();
            acceptTap = false;

            // allow player movement
            PlayerMovement pm = FindObjectOfType<PlayerMovement>();
            pm.moveEnabled = true;
        }
    }

    private IEnumerator BackgroundFade(float start, float end, float time)
    {
        Color c;
        float t = 0;

        while (t < time)
        {
            t += Time.deltaTime;

            float v = Mathf.Lerp(start, end, t / time);

            c = background.color;
            c.a = v;
            background.color = c;

            yield return null;
        }

        c = background.color;
        c.a = end;
        background.color = c;

        StartCoroutine(DialogueFade(start, end, time * 2));

        if (end == 1)
            if (!LevelManager.Instance.InvalidIndex())
                loadingAnim.SetActive(true);
            else
                ToggleLoading();
    }

    private IEnumerator DialogueFade(float start, float end, float time)
    {
        Color c;
        float t = 0;

        while (t < time)
        {
            t += Time.deltaTime;

            float v = Mathf.Lerp(start, end, t / time);

            c = dialogue.color;
            c.a = v;
            dialogue.color = c;

            yield return null;
        }

        c = dialogue.color;
        c.a = end;
        dialogue.color = c;

        if (end == 0)
            gameObject.SetActive(false);
    }
}
