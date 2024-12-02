using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public Image fadeImage; // Reference to the UI Image used for fading.
    public float fadeDuration = 1f; // Time for fade-in/out.
    public string nextSceneName; // Name of the scene to load.

    private void Start()
    {
        //StartCoroutine(FadeIn());
    }

    public void TriggerSceneTransition()
    {
        StartCoroutine(TransitionToNextScene());
    }

    public void TriggerSceneFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;
        float timeDelta = Time.deltaTime;
        for (float t = 0; t <= fadeDuration; t += timeDelta)
        {
            
            color.a = 1 - (t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
        color.a = 0;
        fadeImage.color = color;
        fadeImage.gameObject.SetActive(false);
    }

    private IEnumerator TransitionToNextScene()
    {
        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;

        // Fade out to black
        for (float t = 0; t <= fadeDuration; t += Time.deltaTime)
        {
            color.a = t / fadeDuration;
            fadeImage.color = color;
            yield return null;
        }
        color.a = 1;
        fadeImage.color = color;

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }
}
