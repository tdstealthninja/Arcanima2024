using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerURL : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<VideoPlayer>().url = Path.Combine(Application.streamingAssetsPath, "Cups_Cutscene_1.mp4");
    }
}
