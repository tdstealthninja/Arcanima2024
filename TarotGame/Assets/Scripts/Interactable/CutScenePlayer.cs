using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CutScenePlayer : Interactable
{
    [SerializeField]
    private Canvas c;
    [SerializeField]
    private Image blackout;
    [SerializeField]
    private VideoPlayer video;
    [SerializeField]
    private float fadeDuration;
    [SerializeField]
    private GameObject statueVisual;
    [SerializeField]
    private GameObject pointLight;

    [SerializeField]
    private GameObject empressNPC;

    delegate void Next();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override bool Interact(Interactor interactor)
    {
        c.gameObject.SetActive(true);
        StartCoroutine(BlackoutFade(Color.black, fadeDuration, () =>
        {
            video.gameObject.SetActive(true);
            video.Play();
            StartCoroutine(BlackoutFade(new Color(0, 0, 0, 0), fadeDuration, () =>
            {
                video.loopPointReached += (player) =>
                {
                    StartCoroutine(BlackoutFade(Color.black, fadeDuration, () =>
                    {
                        VideoEnded();
                    }));
                };
            }));
        }));

        return true;
    }

    public void VideoEnded()
    {
        blackout.color = Color.black;
        video.gameObject.SetActive(false);
        // hide statue
        statueVisual.SetActive(false);
        pointLight.SetActive(false);
        // spawn empress npc
        GameObject go = Instantiate(empressNPC);
        go.GetComponent<GridTransform>().ForceSetGridPosition(GetComponent<GridTransform>().GetGridPosition());
        BillboardManager.Instance.UpdateSprites(Camera.main);
        StartCoroutine(BlackoutFade(new Color(0, 0, 0, 0), fadeDuration, () =>
        {
            // destroy statue
            Destroy(gameObject);
        }));
    }

    public override void OnStateChange()
    {
        // nothing happens
    }

    IEnumerator BlackoutFade(Color to, float duration, Next next)
    {
        Color from = blackout.color;
        float time = 0;
        while (time < duration)
        {
            blackout.color = Color.Lerp(from, to, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        blackout.color = to;
        next();
    }
}
