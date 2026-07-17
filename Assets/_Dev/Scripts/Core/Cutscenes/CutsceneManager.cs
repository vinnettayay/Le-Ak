using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    [Header("Objects")]
    public Cutscene_ScriptableObjects cutscenesData;
    public Image firstScene;
    public Image secondScene;
    public Animator fadeAnimator;

    [Header("Variables")]
    public int scene1Index = 0;
    public int scene2Index = 1;
    private bool onTransition = false;
    private bool scene1Displayed = true;
    private bool isFinished = false;
    [SerializeField] private AudioSource bgm;
    [SerializeField] private SceneManagement sceneManagement;
    [SerializeField] private string sceneName;

    void Awake()
    {
        if (fadeAnimator == null) fadeAnimator.GetComponentInChildren<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        firstScene.sprite = cutscenesData.scenes[scene1Index];
        secondScene.sprite = cutscenesData.scenes[scene2Index];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !onTransition)
        {
            Debug.Log(isFinished);
            if (isFinished == false)
            {
                StartCoroutine(FadeImages());
            }
            else
            {
                sceneManagement.LoadAnyScene(sceneName);
            }
        } 
    }

    private IEnumerator FadeImages()
    {
        onTransition = true;

        if (scene1Displayed == true)
        {
            fadeAnimator.SetTrigger("ShowScene2");
            yield return new WaitForSeconds(1f);

            int nextIndex = (scene2Index + 1) % cutscenesData.scenes.Count;
            if (nextIndex == 0)
            {
                isFinished = true;
                onTransition = false;
                yield break;
            }

            scene1Index = nextIndex;

            Debug.Log(scene1Index);
            Debug.Log(scene2Index);

            firstScene.sprite = cutscenesData.scenes[scene1Index];
            secondScene.sprite = cutscenesData.scenes[scene2Index];
        }
        else
        {
            fadeAnimator.SetTrigger("ShowScene1");
            yield return new WaitForSeconds(1f);


            int nextIndex = (scene1Index + 1) % cutscenesData.scenes.Count;
            if (nextIndex == 0)
            {
                sceneManagement.LoadAnyScene(sceneName);
                onTransition = false;
                yield break;
            }
            scene2Index = nextIndex;

            Debug.Log(scene1Index);
            Debug.Log(scene2Index);

            secondScene.sprite = cutscenesData.scenes[scene2Index];
        }

        scene1Displayed = !scene1Displayed;
        onTransition = false;
    }
}
