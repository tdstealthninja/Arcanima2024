using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Rain : MonoBehaviour
{
    [SerializeField, Range(0, 10)]
    private float scale = 1;

    [SerializeField, Range(0, 10)]
    private float fallSpeed = 1;

    private ParticleSystem rainEffect;
    private float rainDefaultRate;
    private ParticleSystem rainSplashEffect;
    private float rainSplashDefaultRate;

    private float defaultFallSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rainEffect = transform.Find("Rain Effect").GetComponent<ParticleSystem>();
        rainDefaultRate = rainEffect.emission.rateOverTimeMultiplier;
        rainSplashEffect = transform.Find("Rain Splash Effect").GetComponent<ParticleSystem>();
        rainSplashDefaultRate = rainSplashEffect.emission.rateOverTimeMultiplier;

        defaultFallSpeed = rainEffect.main.gravityModifier.constant;
    }

#if UNITY_EDITOR
    private void Update()
    {
        UpdateEffect();
    }
#endif

    public void ScaleEffect(float newScale)
    {
        scale = newScale;
        UpdateEffect();
    }

    public void ScaleFallSpeed(float newFallSpeed)
    {
        fallSpeed = newFallSpeed;
        UpdateEffect();
    }

    public void ScaleEffectOverTime(float newScale, float time) => StartCoroutine(SlowChange(fallSpeed, newScale, time));

    private void UpdateEffect()
    {
        // y = x * default
        var rainEmit = rainEffect.emission;
        rainEmit.rateOverTimeMultiplier = scale * rainDefaultRate;

        // y = sqrt(x) * default
        var rainSplashEmit = rainSplashEffect.emission;
        rainSplashEmit.rateOverTimeMultiplier = Mathf.Sqrt(scale) * rainSplashDefaultRate;

        // Fall speed
        var splashMain = rainSplashEffect.main;
        splashMain.gravityModifier = fallSpeed * defaultFallSpeed;
        var rainMain = rainEffect.main;
        rainMain.gravityModifier = fallSpeed * defaultFallSpeed;
    }

    private IEnumerator SlowChange(float startValue, float endValue, float speed)
    {
        float elapsedTime = 0;

        while (elapsedTime < speed)
        {
            ScaleEffect(Mathf.Lerp(startValue, endValue, (elapsedTime / speed)));
            elapsedTime += Time.deltaTime;

            // wait for next frame to update again
            yield return null;
        }

        // make sure to end at correct value
        ScaleEffect(endValue);
        yield return null;
    }
}
