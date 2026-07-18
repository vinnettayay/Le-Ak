using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LanternController : MonoBehaviour
{
    public static LanternController Instance;

    [Header("Light")]
    [SerializeField] private Light lanternLight;

    [Header("UI")]
    [SerializeField] private Image lanternImage;
    [SerializeField] private Sprite lanternOn;
    [SerializeField] private Sprite lanternOff;

    [Header("Fuel")]
    [SerializeField] private float maxFuel = 180f;
    [SerializeField] private float warningDuration = 8f;

    [Header("Flicker")]
    [SerializeField] private float minIntensity = 0.2f;
    [SerializeField] private float maxIntensity = 1f;
    private float currentFuel;
    private bool warningStarted;
    private bool isOff;
    public bool IsOff => isOff;
    public bool NeedsRefill => currentFuel < maxFuel;
    private void Awake() 
    {
        Instance = this;    
    }
    void Start()
    {   
        currentFuel = maxFuel;
        lanternImage.sprite = lanternOn;
        lanternLight.enabled = true;
        lanternLight.intensity = maxIntensity;
    }
    void Update()
    {
        if (isOff) return;
        currentFuel -= Time.deltaTime;
        if (!warningStarted && currentFuel <= warningDuration)
        {
            warningStarted = true;
            StartCoroutine(FlickerRoutine());
        }   
        if (currentFuel <= 0f)
        {
            TurnOff();
        }
    }
    private IEnumerator FlickerRoutine()
    {
        //Play Audio Flickering
        while (!isOff)
        {
            lanternLight.intensity = Random.Range(minIntensity, maxIntensity);
            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        }
    }
    private void TurnOff()
    {
        isOff = true;
        StopAllCoroutines();
        lanternLight.enabled = false;
        lanternImage.sprite = lanternOff;

        //Set the Environment Darker
    }
    public void Refill()
    {
        currentFuel = maxFuel;
        warningStarted = false;
        isOff = false;

        lanternLight.enabled = true;
        lanternLight.intensity = maxIntensity;
        lanternImage.sprite = lanternOn;

        //PlayAudio Refill
    }
    public float FuelPercent()
    {
        return currentFuel / maxFuel;
    }
}
