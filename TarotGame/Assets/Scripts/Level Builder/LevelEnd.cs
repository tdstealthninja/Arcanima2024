using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        PlayLoadedLevelV2 pl = FindObjectOfType<PlayLoadedLevelV2>();
        if (pl != null)
        {
            pl.onLoadEnded.AddListener(() =>
            {
                pl.shouldEndRealm = true;
                Destroy(gameObject);
            });
        }
    }
}
