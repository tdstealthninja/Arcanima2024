using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChangePoofScript : MonoBehaviour
{
    private ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();

        FindObjectOfType<StateChange>().onStateChange.AddListener(() => particles.Play());
    }
}
