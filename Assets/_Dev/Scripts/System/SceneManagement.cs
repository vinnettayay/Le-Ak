using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneManagement : MonoBehaviour
{
    [Header("Fade")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    private bool isLoading;
    void Start()
    {
        StartCoroutine(FadeIn());
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeIn());
    }
    public void LoadAnyScene(string sceneName)
    {
        if (!isLoading) StartCoroutine(FadeOutAndLoad(sceneName));
    }
    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        fadeImage.gameObject.SetActive(true);
        isLoading = true;
        Color color = fadeImage.color;
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }
    private IEnumerator FadeIn()
    {
        Color color = fadeImage.color;
        color.a = 1;
        fadeImage.color = color;

        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
        color.a = 0;
        fadeImage.color = color;
        isLoading = false;
        fadeImage.gameObject.SetActive(false);
    }
    public void QuitGame()
    {
        StartCoroutine(FadeIn());
        Application.Quit();
    }
}
