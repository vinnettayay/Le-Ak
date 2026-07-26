using UnityEngine;
using UnityEngine.Video;

public class CutsceneVideo : MonoBehaviour
{
    [Header("Video")]
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Scene")]
    [SerializeField] private string nextScene;

    [Header("Skip (Optional)")]
    [SerializeField] private bool allowSkip = false;
    [SerializeField] private KeyCode skipKey = KeyCode.Space;
    [SerializeField] private SceneManagement sceneManagement;
    private bool isLoading = false;

    private void Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer missing");
            return;
        }
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    private void Update()
    {
        if (allowSkip && Input.GetKeyDown(skipKey))
        {
            LoadNextScene();
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        if (isLoading) return;
        isLoading = true;

        videoPlayer.loopPointReached -= OnVideoFinished;
        sceneManagement.LoadAnyScene(nextScene);
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached -= OnVideoFinished;
    }
}
