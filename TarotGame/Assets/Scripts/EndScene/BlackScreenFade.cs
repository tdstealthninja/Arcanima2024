using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BlackScreenFade : MonoBehaviour
{

    [SerializeField]
    private float startDelay = 0.0f;

    [SerializeField]
    private Image target;

    // Start is called before the first frame update
    void Start()
    {
       StartCoroutine(FadeImage()); 
    }

    IEnumerator FadeImage()
    {
    
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                target.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
}
