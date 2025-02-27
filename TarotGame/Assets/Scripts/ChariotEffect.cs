using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChariotEffect : MonoBehaviour
{
    ParticleSystem ps;
    bool emitting = false;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        ps.Stop();

        FindObjectOfType<StateChange>().onStateChange.RemoveListener(Toggle);
        FindObjectOfType<StateChange>().onStateChange.AddListener(Toggle);
    }

    private void Toggle()
    {
        emitting = !emitting;
        if (emitting)
            ps.Play();
        else
            ps.Stop();
    }
}
