using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testsound : MonoBehaviour
{
    // Start is called before the first frame update
    private FMOD.Studio.EventInstance flap;



    void Start()
    {
        flap = FMODUnity.RuntimeManager.CreateInstance("event:/test");
    }

    // Update is called once per frame
    void Update()
    {

            FMODUnity.RuntimeManager.PlayOneShot("event:/test");


    }
}
