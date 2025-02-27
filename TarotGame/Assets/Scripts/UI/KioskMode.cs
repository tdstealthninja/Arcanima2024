using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class KioskMode : MonoBehaviour
{
    [SerializeField, Tooltip("in seconds")]
    private int afkDelay = 20;
    [SerializeField]
    private GameObject video;
    private float time = 0;

    private void Start()
    {
        if (!video)
        {
            Debug.LogError("Missing Video");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (video.activeInHierarchy)
            time = 0;
        else
        {
            time += Time.deltaTime;
            if (time >= afkDelay)
            {
                video.SetActive(true);
                video.GetComponent<VideoPlayer>().url = Path.Combine(Application.streamingAssetsPath, "ArcanimaTrailer.mp4");
            }
        }
    }
}
