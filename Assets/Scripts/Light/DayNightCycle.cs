using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DayNightCycle : MonoBehaviour
{
    public Light2D globalLight;
    public Image fadeImage;
    public float dayIntensity;
    public float nightIntensity; 
    public Gradient lightColor;
    public Color nightLightColor = new Color(0.1f, 0.1f, 0.4f); 

    public float fadeDuration; 
    public float transitionDuration; 

    private bool isFading = false; 
    private bool hasFaded = false; 

    private void Start()
    {
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
        TimeManager.timeManager.OnTimeChange += UpdateLighting;
    }

    private void UpdateLighting()
    {
        float currentTime = TimeManager.timeManager.gameTime % 24; 

        if (currentTime < 6f || currentTime > 18f) 
        {
            globalLight.color = nightLightColor; 
        }
        else 
        {
            globalLight.color = lightColor.Evaluate(currentTime / 24); 
        }

        if (currentTime >= 18f && !hasFaded)
        {
            hasFaded = true; 
            StartCoroutine(FadeToBlack());
        }
        else if (currentTime < 6f) 
        {
            if (isFading) return; 
            StartCoroutine(TransitionToDay(currentTime));
        }
        else
        {
            globalLight.intensity = Mathf.Lerp(nightIntensity, dayIntensity, (currentTime - 6f) / 6f);
        }
    }

    private IEnumerator FadeToBlack()
    {
        float startAlpha = fadeImage.color.a;
        float targetAlpha = 1f; 
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime; 
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            yield return null; 
        }

        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, targetAlpha);


        yield return new WaitForSeconds(2f); 

        time = 0; 
        targetAlpha = 0f; 

        while (time < fadeDuration)
        {
            time += Time.deltaTime; 
            float alpha = Mathf.Lerp(1f, targetAlpha, time / fadeDuration);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);
            yield return null; 
        }

        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, targetAlpha);
        isFading = false; 
    }

    private IEnumerator TransitionToDay(float currentTime)
    {
        isFading = true; 
        float time = 0;
        float startIntensity = globalLight.intensity;

        while (time < transitionDuration)
        {
            time += Time.deltaTime; 
            globalLight.intensity = Mathf.Lerp(startIntensity, dayIntensity, time / transitionDuration);
            yield return null; 
        }

        globalLight.intensity = dayIntensity; 
        isFading = false; 
    }

    private void OnDestroy()
    {
        TimeManager.timeManager.OnTimeChange -= UpdateLighting;
    }
}
