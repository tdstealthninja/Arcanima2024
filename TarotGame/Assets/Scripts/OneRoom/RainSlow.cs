using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneRoom
{
    public class RainSlow : MonoBehaviour
    {
        private Rain[] rain;

        private void Start()
        {
            rain = FindObjectsOfType<Rain>();
        }

        public void SlowRain()
        {
            foreach(Rain ra in rain)
            {
                ra.ScaleEffectOverTime(0, 0.5f);
            }
        }
    }
}