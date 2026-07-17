using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private CharacterController controller;

    [Header("Checkpoint")]
    [SerializeField] private Transform currentCheckpoint;

    [Header("EnemyRespawn")]
    [SerializeField] private Transform[] enemySpawnPoints;

    [Header("UI")]
    [SerializeField] private Image blackFade;
    [SerializeField] private Image jumpscareImage;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip jumpscareClip;

    [Header("Timing")]
    [SerializeField] private float jumpscareDuration = 2f;
    [SerializeField] private float fadeSpeed = 2f;
    private bool gameOver;
    private void Awake() 
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);    
    }
    void Start()
    {
        blackFade.gameObject.SetActive(true);
        jumpscareImage.gameObject.SetActive(false);

        SetImageAlpha(blackFade, 0f);
    }
    public void PlayerCaught(EnemyAIBehaviour enemy)
    {
        if (gameOver) return;

        StartCoroutine(GameOverRoutine(enemy));
    }
    public void SetCheckpoint(Transform checkpoint)
    {
        currentCheckpoint = checkpoint;
        Debug.Log("Checkpoint Updated : " + checkpoint.name);
    }
    private IEnumerator GameOverRoutine(EnemyAIBehaviour enemy)
    {
        gameOver = true;
        playerMovement.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("Jumpscare : Here we are");
        jumpscareImage.gameObject.SetActive(true);
        if (audioSource != null && jumpscareClip != null) audioSource.PlayOneShot(jumpscareClip);

        yield return new WaitForSeconds(jumpscareDuration);
        Debug.Log("Jumpscare : Is at it");
        yield return StartCoroutine(Fade(0f, 1f));

        jumpscareImage.gameObject.SetActive(false);
        Debug.Log("Jumpscare : Done");

        controller.enabled = false;
        player.position = currentCheckpoint.position;
        player.rotation = currentCheckpoint.rotation;
        controller.enabled = true;  

        if (enemySpawnPoints.Length > 0)
        {
            int random = Random.Range(0, enemySpawnPoints.Length);
            enemy.ResetEnemy(enemySpawnPoints[random].position);
        }
        playerMovement.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yield return StartCoroutine(Fade(1f, 0));
        gameOver = false;
    }
    private IEnumerator Fade(float start, float end)
    {
        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime * fadeSpeed;
            float alpha = Mathf.Lerp(start, end, time);
            SetImageAlpha(blackFade, alpha);
            yield return null;
        }
        SetImageAlpha(blackFade, end);
    }
    private void SetImageAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}
